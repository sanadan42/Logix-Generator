using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace LogixGenerator
{

    public struct AILimits
    {
        public string Low, High;
    }

    struct AICfg
    {
        public AILimits PV, Raw, Fail;
        public string Hysterisis;
    }

    public class AnalogInput : Tag
    {
        private const string TAB = "\t";

        //private string Raw, PV, mA, Units, Hysterisis;
        //private bool Fault;
        private AICfg CFG;
        private AlarmData HH, H, L, LL, Failed;
        private string Units, Hysterisis;

        public AnalogInput()
        {
            HH = new AlarmData();
            H = new AlarmData();
            L = new AlarmData();
            LL = new AlarmData();
            Failed = new AlarmData();
            base.tagPrefix = "ai_";
    }

        public override string Address
        {
            get
            {
                string channel = GetChannelBasedOnModuleType();
                return string.Format(LogixGenerator.Properties.Resources.addressAI, this.Rack, this.Slot, channel);
            }
        }

        public override string TagDef
        {
            get
            {
                if (!this.IsEmpty)
                {
                    // {0} : DUCO_AI_DATA:= [0.0e+0, 0.0e+0, 0.0e+0, 0,[{ 1},{ 2},{ 3},{ 4}],{ 5},{ 6},{ 7},{ 8},{ 9}];
                    //
                    //{ 0} = Tag Name,
                    //{ 1} = PV min and Max,
                    //{ 2} = Raw Min and Max,
                    //{ 3} = Failed Min and Max,
                    //{ 4} = Hysterisis,
                    //{ 5} = HH,
                    //{ 6} = H,
                    //{ 7} = L,
                    //{ 8} = LL,
                    //{ 9} = Failed
                    string s;
                    s = TAB + TAB + string.Format(LogixGenerator.Properties.Resources.tagAI, this.TagName, Limits(this.CFG.PV), Limits(this.CFG.Raw), Limits(this.CFG.Fail), this.Hysterisis, base.AlarmData(HH), base.AlarmData(H), base.AlarmData(L), base.AlarmData(LL), base.AlarmData(Failed), this.Description) + System.Environment.NewLine;
                    s += TAB + TAB + string.Format(LogixGenerator.Properties.Resources.tagAIAOI, this.TagName, this.Description) + System.Environment.NewLine;
                    return s;
                }
                else
                {
                    return null;
                }
            }
        }

        public string HMIAlarm(string type)
        {
            return this.TagName + "." + type + ".InvLatched";
        }

        public string HMIFOAlarm(string type)
        {
            return this.TagName + "." + type + ".InvFO";
        }

        public bool AlarmEnabled(string type)
        {
            bool result = false;
            switch (type.ToLower())
            {
                case "hh":
                    result = this.HH.Enable != "0";
                    break;
                case "h":
                    result = this.H.Enable != "0";
                    break;
                case "l":
                    result = this.L.Enable != "0";
                    break;
                case "ll":
                    result = this.LL.Enable != "0";
                    break;
                case "failed":
                    result = this.Failed.Enable != "0";
                    break;
                default:
                    break;
            }
            return result;
        }

        public bool FOEnabled(string type)
        {
            bool result = false;
            switch (type.ToLower())
            {
                case "hh":
                    result = this.HH.Enable == "3";
                    break;
                case "h":
                    result = this.H.Enable == "3";
                    break;
                case "l":
                    result = this.L.Enable == "3";
                    break;
                case "ll":
                    result = this.LL.Enable == "3";
                    break;
                case "failed":
                    result = this.Failed.Enable == "3";
                    break;
                default:
                    break;
            }
            return result;
        }


        public override string Logic
        {
            get { return string.Format(LogixGenerator.Properties.Resources.logic_AI, this.Address, this.TagName); }
        }

        public override void Populate(DataRow row)
        {
            base.Populate(row);

            this.Units = row[AIColumnKeys.Units].ToString();
            this.CFG.PV.Low = StringHelp.StringCheckZero(row[AIColumnKeys.EULow].ToString());
            this.CFG.PV.High = StringHelp.StringCheckZero(row[AIColumnKeys.EUHigh].ToString());

            this.LL.SP = StringHelp.StringCheckZero(row[AIColumnKeys.LLSP].ToString());
            this.LL.Enable = row[AIColumnKeys.LLEnable].ToString();
            this.LL.Debounce = row[AIColumnKeys.LLDeb].ToString();

            this.L.SP = StringHelp.StringCheckZero(row[AIColumnKeys.LSP].ToString());
            this.L.Enable = row[AIColumnKeys.LEnable].ToString();
            this.L.Debounce = row[AIColumnKeys.LDeb].ToString();

            this.H.SP = StringHelp.StringCheckZero(row[AIColumnKeys.HSP].ToString());
            this.H.Enable = row[AIColumnKeys.HEnable].ToString();
            this.H.Debounce = row[AIColumnKeys.HDeb].ToString();

            this.HH.SP = StringHelp.StringCheckZero(row[AIColumnKeys.HHSP].ToString());
            this.HH.Enable = row[AIColumnKeys.HHEnable].ToString();
            this.HH.Debounce = row[AIColumnKeys.HHDeb].ToString();

            // default configuration data
            //default values for failed alarm
            switch (base.ModuleType)
            {
                case (int)ProcessorSeries.Series1769:
                    CFG.Raw.Low = "4000.0";
                    CFG.Raw.High = "20000.0";
                    break;
                case (int)ProcessorSeries.Series1756:
                default:
                    CFG.Raw.Low = "4.0";
                    CFG.Raw.High = "20.0";
                    break;
            }

            CFG.Hysterisis = "0.0";
            this.CFG.Fail.High = "20.5";
            this.CFG.Fail.Low = "3.6";
            this.Failed.Enable = "alarm"; // this is the default to enable the alarm but not the FO

            // default value for hysterisis, unless we add this to the spreadsheet
            this.Hysterisis = "0.0";
        }

        private string Limits(AILimits limit)
        {
            return "[" + limit.Low + "," + limit.High + "]";
        }

    }

}
