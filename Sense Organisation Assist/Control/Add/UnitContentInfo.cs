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

namespace IxSApp
{
    /// <summary>
    /// 
    /// </summary>
    public class UnitContentInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitContentInfo"/> class.
        /// </summary>
        public UnitContentInfo() { }

        /// <summary>
        /// The id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// The table name
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// The item
        /// </summary>
        public Units Item { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can selected.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can selected; otherwise, <c>false</c>.
        /// </value>
        public bool CanSelect { get; set; }
    }
}
