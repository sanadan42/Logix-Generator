using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace LogixGenerator
{
    enum ProcessorSeries : int
    {
        Series1769 = 0,
        Series1756 = 1
    }

    class Controller
    {
        private const string TAB = "\t";
        private string cr = System.Environment.NewLine + System.Environment.NewLine;

        private List<AnalogInput> aiList;
        private List<AnalogOutput> aoList;
        private List<DiscreteInput> diList;
        private List<DiscreteOutput> doList;
        private string sControllerName, sControllerType;


        public string Name
        {
            get { return this.sControllerName; }
            set { this.sControllerName = value; }
        }

        public string ProcessorType
        {
            get { return this.sControllerType; }
            set { this.sControllerType = value; }
        }

        public string ControllerDef
        {
            get { return string.Format(LogixGenerator.Properties.Resources.ctl_Controller_Definition, this.sControllerName, this.sControllerType) + System.Environment.NewLine; }
        }

        public string HeaderDef
        {
            get { return LogixGenerator.Properties.Resources.header; }
        }

        public string CompleteProgram
        {
            get
            {
                UserDefinedTypes udt = new UserDefinedTypes();
                AddOnInstructions aoi = new AddOnInstructions();

                string program;
                program = this.HeaderDef + cr + this.ControllerDef + cr;
                program += udt.UserTypes + cr;
                program += aoi.AddOnInstructionDefs + cr;
                program += CreateTags() + cr;
                program += CreateInputProgram() + cr;
                program += CreateOutputProgram() + cr;
                program += CreateTasks() + cr;
                program += LogixGenerator.Properties.Resources.controllerFooter;
                return program;
            }
        }

        // populate lists
        public void PopulateData(DataSet ds)
        {
            //probably should do some try catches in here - but fuck it
            if (ds != null)
            {
                this.BuildList(ref aiList, ds.Tables["AI$"]); // populate AI list without the need to create a dataTABle for AI
                this.BuildList(ref diList, ds.Tables["DI$"]); // populate AI list without the need to create a dataTABle for AI
                this.BuildList(ref doList, ds.Tables["DO$"]); // populate AI list without the need to create a dataTABle for AI
                this.BuildList(ref aoList, ds.Tables["AO$"]); // populate AI list without the need to create a dataTABle for AI
            }
        }

        
        // trying to create a generic method as this code will be exactly the same for each of the different data types
        // the only change will be the implementation of the Populate function
        private void BuildList<T>(ref List<T> list, DataTable dt) where T : Tag, new()
        {
            if (dt != null)
            {
                int spareCount = 0;
                list = new List<T>();
                string processor = ProcessorType.Substring(0, 4);
                foreach (DataRow row in dt.Rows)
                {
                    // if the rack is empty then the row is ignored
                    // if the channel is empty then the row is ignored
                    // if the slot is empty then the row is ignored
                    bool ignore = row[TagColumnKeys.Rack].ToString() == "" || row[TagColumnKeys.Slot].ToString() == "" || row[TagColumnKeys.Channel].ToString() == "";
                    if (!ignore)
                    {
                        T data = new T();

                        // need to set the module type before populating so that channel can get set correctly
                        data.ModuleType = GetProcessorType(processor);
                        data.Populate(row);

                        if (data.IsEmpty)
                        {
                            spareCount++;
                            data.TagName = "Spare" + spareCount.ToString();
                            data.IsEmpty = false;
                        }
                        list.Add(data);
                    }
                }
            }
        }

        public string CreateHMIAlarmXML()
        {
            string triggers = "";
            string messages = "";
            string xmlString = "";
            int count = 0;

            foreach (DiscreteInput di in this.diList)
            {
                BuildDIHMITrigger(0, di, ref count, ref triggers, ref messages);
                BuildDIHMITrigger(1, di, ref count, ref triggers, ref messages);
            }

            foreach (AnalogInput ai in this.aiList)
            {
                BuildAIHMITrigger("HH", 0, ai, ref count, ref triggers, ref messages);
                BuildAIHMITrigger("H", 0, ai, ref count, ref triggers, ref messages);
                BuildAIHMITrigger("L", 0, ai, ref count, ref triggers, ref messages);
                BuildAIHMITrigger("LL", 0, ai, ref count, ref triggers, ref messages);
                BuildAIHMITrigger("Failed", 0, ai, ref count, ref triggers, ref messages);

                BuildAIHMITrigger("HH", 1, ai, ref count, ref triggers, ref messages);
                BuildAIHMITrigger("H", 1, ai, ref count, ref triggers, ref messages);
                BuildAIHMITrigger("L", 1, ai, ref count, ref triggers, ref messages);
                BuildAIHMITrigger("LL", 1, ai, ref count, ref triggers, ref messages);
                BuildAIHMITrigger("Failed", 1, ai, ref count, ref triggers, ref messages);
            }

            xmlString = LogixGenerator.Properties.Resources.hmiFileHeader + cr;
            xmlString += LogixGenerator.Properties.Resources.hmiAlarmsHeader + cr;
            xmlString += LogixGenerator.Properties.Resources.hmiAlarmHeader + cr;
            xmlString += LogixGenerator.Properties.Resources.hmiTriggersHeader + cr;
            xmlString += triggers;
            xmlString += LogixGenerator.Properties.Resources.hmiTriggersFooter + cr;
            xmlString += LogixGenerator.Properties.Resources.hmiMessagesHeader + cr;
            xmlString += messages;
            xmlString += LogixGenerator.Properties.Resources.hmiMessagesFooter + cr;
            xmlString += LogixGenerator.Properties.Resources.hmiAlarmFooter + cr;
            xmlString += LogixGenerator.Properties.Resources.hmiAlarmsFooter + cr;

            return xmlString;
        }
        
        // 
        private void BuildDIHMITrigger(int NormalOrFirstOut, DiscreteInput di, ref int count, ref string trigs, ref string mess)
        {
            if (NormalOrFirstOut == 0)
            {
                if (di.AlarmEnabled)
                {
                    count++;
                    string alarmMessage = di.Description + " Alarm";
                    string backColor = "#800000";
                    string foreColor = "#FFFFFF";
                    BuildTriggerHelper(count, ref trigs, ref mess, di.HMIAlarm, alarmMessage, backColor, foreColor);
                }
            }
            else if (NormalOrFirstOut == 1)
            {
                if (di.FOEnabled)
                {
                    count++;
                    string backColor = "#000000";
                    string foreColor = "#FF0000";
                    string alarmMessage = di.Description + " First Out";
                    BuildTriggerHelper(count, ref trigs, ref mess, di.HMIFOAlarm, alarmMessage, backColor, foreColor);
                }
            }
        }

        private void BuildAIHMITrigger(string type, int NormalOrFirstOut, AnalogInput ai, ref int count, ref string trigs, ref string mess)
        {
            if (NormalOrFirstOut == 0)
            {
                if (ai.AlarmEnabled(type))
                {
                    count++;
                    string foreColor = "#FFFFFF";
                    string backColor = "#800000";
                    string alarmMessage = ai.Description + " " + type.ToUpper() + " Alarm";
                    BuildTriggerHelper(count, ref trigs, ref mess, ai.HMIAlarm(type), alarmMessage, backColor, foreColor);
                }
            }
            else if(NormalOrFirstOut == 1)
            {
                if (ai.FOEnabled(type))
                {
                    count++;
                    string backColor = "#000000";
                    string foreColor = "#FF0000";
                    string alarmMessage = ai.Description + " " + type.ToUpper() + " First Out";
                    BuildTriggerHelper(count, ref trigs, ref mess, ai.HMIFOAlarm(type), alarmMessage, backColor, foreColor);
                }
            }
        }

        private void BuildTriggerHelper(int count, ref string trigs, ref string mess, string tagName, string alarmMessage, string backColor, string foreColor)
        {
            string triggerID = "T" + count.ToString();
            string messageID = "M" + count.ToString();
            string labelID = tagName.Replace(".", "_");
            trigs += string.Format(LogixGenerator.Properties.Resources.hmiTriggers, triggerID, tagName, labelID) + cr;
            mess += string.Format(LogixGenerator.Properties.Resources.hmiMessages, messageID, triggerID, alarmMessage, backColor, foreColor) + cr;
        }

        private string CreateTags()
        {
            string tags;
            //opening 
            tags = TAB + "TAG" + System.Environment.NewLine;
            tags += GeneralTags();
            tags += WriteTags(diList);
            tags += WriteTags(aiList);
            tags += WriteTags(doList);
            tags += WriteTags(aoList);

            //closing
            tags += TAB + "END_TAG" + System.Environment.NewLine;

            return tags;
        }
        private string GeneralTags()
        {
            string generalTags;
            generalTags = TAB + TAB + "ALARMS : " + AddOnInstructions.ALARM_BITS_TYPE_NAME + " := [0,0];" + System.Environment.NewLine;
            generalTags += TAB + TAB + "MASTER_ALARMS : " + AddOnInstructions.ALARM_CTL_FUNCTION_NAME + " := [3];" + System.Environment.NewLine;
            generalTags += TAB + TAB + "udt_Clock : " + AddOnInstructions.DUCO_CLOCK + " := [0,0,0,0,0,0,0,0];" + System.Environment.NewLine;

            return generalTags;
        }

        private string CreateInputProgram()
        {
            string inputProgram = "";
           
            inputProgram += LogixGenerator.Properties.Resources.logic_ProgramHeader + cr;
            inputProgram += CreateMainRoutineInputs() + cr;
            inputProgram += CreateProgramConfigRoutine() + cr;
            inputProgram += CreateLogic(LogixGenerator.Properties.Resources.logicLadderHeader + LogixGenerator.Properties.Resources.logicDIName, this.diList) + cr;
            inputProgram += CreateLogic(LogixGenerator.Properties.Resources.logicLadderHeader + LogixGenerator.Properties.Resources.logicAIName, this.aiList) + cr;
            inputProgram += TAB + "END_PROGRAM";

            return inputProgram;
        }

        private string CreateOutputProgram()
        {
            string inputProgram = "";

            inputProgram += LogixGenerator.Properties.Resources.logic_OutputConditioningHeader + cr;
            inputProgram += CreateMainRoutineOutputs() + cr;
            inputProgram += CreateLogic(LogixGenerator.Properties.Resources.logicLadderHeader + LogixGenerator.Properties.Resources.logicDOName, this.doList) + cr;
            inputProgram += CreateLogic(LogixGenerator.Properties.Resources.logicLadderHeader + LogixGenerator.Properties.Resources.logicAOName, this.aoList) + cr;
            inputProgram += TAB + "END_PROGRAM";

            return inputProgram;
        }

        private string CreateTasks()
        {
            string tasks = "";
            tasks += LogixGenerator.Properties.Resources.task_FastTask + System.Environment.NewLine;
            return tasks;
        }

        private string CreateMainRoutineInputs()
        {
            string diLogic, aiLogic;
            diLogic = LogixGenerator.Properties.Resources.logicDIName;
            aiLogic = LogixGenerator.Properties.Resources.logicAIName;
            return string.Format(LogixGenerator.Properties.Resources.logic_Main + System.Environment.NewLine, diLogic, aiLogic);
        }

        private string CreateMainRoutineOutputs()
        {
            string doLogic, aoLogic;
            doLogic = LogixGenerator.Properties.Resources.logicDOName;
            aoLogic = LogixGenerator.Properties.Resources.logicAOName;
            return string.Format(LogixGenerator.Properties.Resources.logic_Main_Outputs + System.Environment.NewLine, doLogic, aoLogic);
        }

        private string CreateProgramConfigRoutine()
        {
            return LogixGenerator.Properties.Resources.logic_ProgramCfg + System.Environment.NewLine;
        }

        private string CreateLogic<T>(string header, List<T> tagList) where T : Tag
        {
            string logic;
            string tabsHdr = TAB + TAB + TAB;
            string footer = LogixGenerator.Properties.Resources.logicFooter;

            logic = tabsHdr + header + System.Environment.NewLine;
            logic += WriteLogic(tagList);
            logic += tabsHdr + footer;

            return logic;
        }
        
        // learning about generics
        // i'm probably going to need to delete this as it doesn't really do anything
        // or can i reuse a similar idea to write logic for each type? define tags for each type?
        private string WriteTags<T>(List<T> list) where T : Tag
        {
            string tags = "";
            if (list != null)
            {
                foreach (T data in list)
                {
                    tags += data.TagDef;
                }
            }
            return tags;
        }

        private string WriteLogic<T>(List<T> list) where T : Tag
        {
            string logic = "";
            if (list != null)
            {
                foreach (T data in list)
                {
                    logic += data.Logic + System.Environment.NewLine;
                }
            }
            return logic;
        }

        private int GetProcessorType(string processor)
        {
            int type = 0;
            switch (processor)
            {
                case "1769":
                    type = (int)ProcessorSeries.Series1769;
                    break;

                case "1756":
                default:
                    type = (int)ProcessorSeries.Series1756;
                    break;
            }
            return type;
        }
    }
}
