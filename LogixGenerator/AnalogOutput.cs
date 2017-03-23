using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace LogixGenerator
{
    class AnalogOutput : Tag
    {
        private const string TAB = "\t";

        public AnalogOutput()
        {
            base.tagPrefix = "ao_";
        }

        public override string Address
        {
            get { return string.Format(LogixGenerator.Properties.Resources.addressAO, this.Rack, this.Slot, this.Channel); }
        }

        //public override void Populate(DataRow row)
        //{
        //    base.Populate(row);
        //}

        public override string Logic
        {
            get
            {
                string s;
                switch(ModuleType)
                {
                    case (int)ProcessorSeries.Series1769:
                        s = string.Format(LogixGenerator.Properties.Resources.logic_AO1769, this.TagName, this.Address);
                        break;
                    case (int)ProcessorSeries.Series1756:
                    default:
                        s = string.Format(LogixGenerator.Properties.Resources.logic_AO, this.TagName, this.Address);
                        break;
                }
                return s;
            }
        }

        public override string TagDef
        {

            get
            {
                if (!this.IsEmpty)
                {
                    string s = TAB + TAB + string.Format(LogixGenerator.Properties.Resources.tagAO, this.TagName, this.Description) + System.Environment.NewLine;

                    if (ModuleType == (int)ProcessorSeries.Series1769)
                    {
                        s += TAB + TAB + string.Format(LogixGenerator.Properties.Resources.tagAOSCP, this.TagName) + System.Environment.NewLine;
                    }
                    
                    return s;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
