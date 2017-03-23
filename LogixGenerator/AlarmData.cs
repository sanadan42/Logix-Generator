using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogixGenerator
{
    public class AlarmData
    {
        // I think that this class needs to get cleaned up - lots of little pieces of code commented out
        // I put them in, just in case I needed them, but the further along I get the more i see that they aren't required


        //public bool AlarmValue, Bypass;
        //public string BypassPreset, BypassMaxTime;
        //private string sEnable, sFOEnable;

        private string sSP, sDebounce;
        private string sEnableCombined;

        // need a constructor to initialize the strings
        public AlarmData()
        {
            // initialize all strings to empty? no - initialize to 0
            sEnableCombined = sSP = sDebounce = "0";
        }

        public string Enable
        {
            get
            {
                return this.sEnableCombined;
            }
            set
            {

                switch (value.ToLower())
                {
                    case "alarm":
                        this.sEnableCombined = "1";
                        break;
                    case "sd":
                        this.sEnableCombined = "3";
                        break;
                    case "disable":
                        this.sEnableCombined = "0";
                        break;
                    default:
                        this.sEnableCombined = "0";
                        break;
                }
            }
        }

        public string SP
        {
            get { return this.sSP; }
            set
            {
                this.sSP = StringHelp.StringCheckZero(value);
            }
        }

        public string Debounce
        {
            get { return this.sDebounce; }
            set { this.sDebounce = StringHelp.StringCheckZero(value); }
        }

        // if i need to then i will implement properties, but for now this is just an easy way to store data
        //
        //public bool Alarm
        //{
        //    get {return AlarmValue; }
        //    set { AlarmValue = value; }
        //}

    }
}
 