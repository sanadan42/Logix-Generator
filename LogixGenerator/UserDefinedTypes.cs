using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogixGenerator
{
    class UserDefinedTypes
    {
        private string cr = System.Environment.NewLine + System.Environment.NewLine;

        public string UserTypes
        {
            get
            {
                string s;
                s = AIType + cr;
                s += AlarmType + cr;
                s += AlarmBitsType + cr;
                s += ClockType + cr;
                s += DIType + cr;
                s += LimitsType + cr;
                s += AIConfig + cr;
                s += MotorData + cr;
                s += LeadLagSP + cr;
                s += LeadLagInterface + cr;
                s += PIDEInterface + cr;
                return  s;
            }
        }
        public string AIType
        {
            get { return LogixGenerator.Properties.Resources.udt_Duco_AI_Data; }
        }
        public string AlarmType
        {
            get { return LogixGenerator.Properties.Resources.udt_Duco_Alarm_Bits; }
        }
        public string AlarmBitsType
        {
            get { return LogixGenerator.Properties.Resources.udt_Duco_Alm; }
        }
        public string ClockType
        {
            get { return LogixGenerator.Properties.Resources.udt_Duco_Clock; }
        }
        public string DIType
        {
            get { return LogixGenerator.Properties.Resources.udt_Duco_DI_Data; }
        }
        public string LimitsType
        {
            get { return LogixGenerator.Properties.Resources.udt_Duco_Limits; }
        }
        public string AIConfig
        {
            get { return LogixGenerator.Properties.Resources.udt_Duco_AI_CFG; }
        }

        public string MotorData
        {
            get { return LogixGenerator.Properties.Resources.udt_Duco_Motor; }
        }
        public string LeadLagSP
        {
            get { return LogixGenerator.Properties.Resources.udt_DUCO_LeadLag_SP; }
        }
        public string LeadLagInterface
        {
            get { return LogixGenerator.Properties.Resources.udt_Duco_LeadLag_Interface; }
        }
        public string PIDEInterface
        {
            get { return LogixGenerator.Properties.Resources.udt_Duco_PIDE_Interface; }
        }

    }
}
