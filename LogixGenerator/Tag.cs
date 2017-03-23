using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace LogixGenerator
{
    
    public class Tag
    {

        protected int iProcessorType;
        protected string sAddress, sName, sDescription, sDefaultValue, sType;
        protected string sRack, sSlot, sChannel;
        protected string sTag;
        protected string sLoopType, sLoopNumber, sLoopSuffix;
        protected bool bIsEmpty;
        protected string tagPrefix;

        public Tag()
        {
            tagPrefix = "";
            IsEmpty = true;
        }

        public bool IsEmpty
        {
            get { return bIsEmpty; }
            set { bIsEmpty = value; }
        }

        public virtual int ModuleType
        {
            get { return iProcessorType; }
            set { this.iProcessorType = value; }
        }

        public virtual string Address
        {
            get { return sAddress; } // i probably will need to convert rack slot channel into an address and then return it
        }

        public string Description
        {
            get { return sDescription; }
            set { sDescription = value; }
        }

        public string DefaultValue
        {
            get { return sDefaultValue;}
            set { sDefaultValue = value; }
        }

        public string DataType
        {
            get { return sType; }
            set { sType = value; }
        }

        public string Rack
        {
            get { return sRack; }
            set { sRack = value; }
        }
        public string Slot
        {
            get { return sSlot; }
            set { sSlot = value; }
        }

        public string Channel
        {
            get { return sChannel; }
            set
            {
                sChannel = value;
                
            }
        }

        public string LoopType
        {
            get { return sLoopType; }
            set { sLoopType = value; }
        }

        public string LoopNumber
        {
            get { return sLoopNumber; }
            set { sLoopNumber = value; }
        }

        public string LoopSuffix
        {
            get { return sLoopSuffix; }
            set { sLoopSuffix = value; }
        }

        public virtual string TagName
        {
            get { return tagPrefix + sTag; }
            set
            {
                sTag = value.Replace("-", "");
                IsEmpty = sTag == "";
            }
        }

        public virtual string TagDef
        {
            get
            {
                return "";
            }
        }

        public virtual string Logic
        {
            get { return ""; }
        }

        public virtual void Populate(DataRow row)
        {
            this.Rack = row[TagColumnKeys.Rack].ToString();
            this.Slot = row[TagColumnKeys.Slot].ToString();
            this.Channel = row[TagColumnKeys.Channel].ToString();
            this.LoopType = row[TagColumnKeys.LoopType].ToString();
            this.LoopNumber = row[TagColumnKeys.LoopNumber].ToString();
            this.LoopSuffix = row[TagColumnKeys.LoopSuffix].ToString();
            this.TagName = row[TagColumnKeys.Tag].ToString();
            this.Description = row[TagColumnKeys.Description].ToString();
        }

        protected string AlarmData(AlarmData alm)
        {
            return string.Format(LogixGenerator.Properties.Resources.alarmData, alm.Enable, alm.Debounce, alm.SP);
        }

        protected bool AlarmEnable(AlarmData alm)
        {
            return alm.Enable.ToLower() == "";
        }

        protected string GetChannelBasedOnModuleType()
        {
            string channel;
            switch (ModuleType)
            {
                case (int)ProcessorSeries.Series1769:
                    try
                    {
                        channel = (Int32.Parse(Channel) < 10) ? "0" + Channel : Channel;
                    }
                    catch (System.FormatException) // the channel cannot be parsed to a number so set the channel = empty string
                    {
                        channel = "";
                    }
                    break;
                case (int)ProcessorSeries.Series1756:
                default:
                    channel = this.Channel;
                    break;
            }
            
            return channel;
        }
    }
}
