using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogixGenerator
{
    class AddOnInstructions
        // small edit so that i can do a push
        // and another
    {
        private string cr = System.Environment.NewLine + System.Environment.NewLine;

        public string AddOnInstructionDefs
        {
            get { return SCPAOI + cr + AlarmAOI + cr + AIAOI + cr + DIAOI + cr + AlarmCTL + cr + HighLowSelect + cr + SPRamp + cr + RunHours; }
        }

        public string AIAOI
        {
            get { return LogixGenerator.Properties.Resources.aoi_AI; }
        }

        public string AlarmAOI
        {
            get { return LogixGenerator.Properties.Resources.aoi_Alarm; }
        }

        public string DIAOI
        {
            get { return LogixGenerator.Properties.Resources.aoi_DI; }
        }

        public string SCPAOI
        {
            get { return LogixGenerator.Properties.Resources.aoi_SCP; }
        }

        public string AlarmCTL
        {
            get { return LogixGenerator.Properties.Resources.aoi_Alarm_CTL; }
        }

        public string HighLowSelect
        {
            get { return LogixGenerator.Properties.Resources.aoi_High_Low_Select; }
        }

        public string SPRamp
        {
            get { return LogixGenerator.Properties.Resources.aoi_Duco_SP_Ramp; }
        }

        public string RunHours
        {
            get { return LogixGenerator.Properties.Resources.aoi_Run_Hours; }
        }

        public string TimedBypass
        {
            get { return LogixGenerator.Properties.Resources.aoi_TimedBypass; }
        }
    }
}
