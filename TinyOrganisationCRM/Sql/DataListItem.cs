using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace As
{
    public class DataListItem
        : ListViewItem
    {
        public DataListItem()
            : base()
        {

        }

        public DataListItem(string text)
            : base(text)
        {

        }

        public DataPackage DataItem { get; set; }
        public Control EntryControl { get; set; }
    }
}
