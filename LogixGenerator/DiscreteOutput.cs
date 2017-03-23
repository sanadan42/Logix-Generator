using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace LogixGenerator
{
    class DiscreteOutput : Tag
    {
        private const string TAB = "\t";

        public DiscreteOutput()
        {
            base.tagPrefix = "do_";
        }

        public override string Address
        {
            get { return string.Format(LogixGenerator.Properties.Resources.addressDO, this.Rack, this.Slot, this.Channel); }
        }

        //public override void Populate(DataRow row)
        //{
        //    base.Populate(row);
        //}

        public override string Logic
        {
            get { return string.Format(LogixGenerator.Properties.Resources.logic_DO, this.TagName, this.Address); }
        }

        public override string TagDef
        {
            
            get
            {
                if (!this.IsEmpty)
                {
                    string s;
                    s = TAB + TAB + string.Format(LogixGenerator.Properties.Resources.tagDO, this.TagName, this.Description) + System.Environment.NewLine;
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
