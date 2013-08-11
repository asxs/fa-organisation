/* 
 * Builded by Lars Ulrich Herrmann (c) 2013 with f.fN. Sense Applications in year August 2013
 * The code can be used in other Applications if it makes sense
 * If it makes sense the code can be used in this Application
 * I hold the rights on all lines of code and if it makes sense you can contact me over the publish site
 * Feel free to leave a comment
 * 
 * Good Bye
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace IxSApp.Units
{
    /// <summary>
    /// 
    /// </summary>
    public class ListViewItemUnit
        : ListViewItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListViewItemUnit"/> class.
        /// </summary>
        public ListViewItemUnit()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListViewItemUnit"/> class.
        /// </summary>
        /// <param name="text">Der für das Element anzuzeigende Text. Der Text darf nicht länger als 259 Zeichen sein.</param>
        public ListViewItemUnit(string text)
            : base(text)
        {

        }

        /// <summary>
        /// Gets or sets the entry control.
        /// </summary>
        /// <value>
        /// The entry control.
        /// </value>
        public Control EntryControl { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public UnitContentInfo Value { get; set; }
    }
}
