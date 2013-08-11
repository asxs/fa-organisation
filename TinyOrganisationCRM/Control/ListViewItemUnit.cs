﻿/* --- Builded by Lars Ulrich Herrmann (c) 2013 with f.fN. Sense Applications in year August 2013
 * The code can be used in other Applications if it makes sense
 * If it makes sense the code can be used in this Application
 * I hold the rights on all lines of code and if it makes sense you can contact me over the publish site
 
 * Feel free to leave a comment
 * --- End
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace IxSApp.Units
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
