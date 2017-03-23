using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogixGenerator
{
    class DiscreteInput : Tag
    {
        private const string TAB = "\t";
        private AlarmData alarm;

        public DiscreteInput()
        {
            base.tagPrefix = "di_";
            this.alarm = new AlarmData();
        }

        public override string Address
        {
            get
            {
                string channel = GetChannelBasedOnModuleType();
                return string.Format(LogixGenerator.Properties.Resources.addressDI, this.Rack, this.Slot, channel);
            }
        }

        public string HMIAlarm
        {
            get
            {
                return this.TagName + ".Alarm." + "InvLatched";
            }
        }
        public string HMIFOAlarm
        {
            get
            {
                return this.TagName + ".Alarm." + "InvFO";
            }
        }
        public bool AlarmEnabled
        {
            get { return this.alarm.Enable != "0"; }
        }
        public bool FOEnabled
        {
            get { return this.alarm.Enable == "3"; }
        }

        public override void Populate(DataRow row)
        {
            base.Populate(row);

            // need something more than this in the future.
            try
            {
                this.alarm.Enable = row[DIColumnKeys.Enable].ToString();
            }
            catch (System.ArgumentException e)
            {
                MessageBox.Show("Error with alarm configuration header for DI object. Don't change the headers!" + e.Message);
            }
            
            this.alarm.Debounce = row[DIColumnKeys.Deb].ToString();
        }

        public override string TagDef
        {
            get
            {
                if (!this.IsEmpty)
                {
                    string s;
                    s = TAB + TAB + string.Format(LogixGenerator.Properties.Resources.tagDI, this.TagName, AlarmData(this.alarm), this.Description) + System.Environment.NewLine;
                    s += TAB + TAB + string.Format(LogixGenerator.Properties.Resources.tagDIAOI, this.TagName, this.Description) + System.Environment.NewLine;
                    return s;
                }
                else
                {
                    return null;
                }
            }
        }

        public override string Logic
        {
            get { return string.Format(LogixGenerator.Properties.Resources.logic_DI, this.Address, this.TagName); }
        }
    }
}
