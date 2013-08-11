using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace As
{
    public class ListViewItemUnit
        : ListViewItem
    {
        public ListViewItemUnit()
            : base()
        {

        }

        public ListViewItemUnit(string text)
            : base(text)
        {

        }

        public Control EntryControl { get; set; }
        public UnitContentInfo Value { get; set; }
    }
}
