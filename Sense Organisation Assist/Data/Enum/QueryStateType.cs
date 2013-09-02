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

namespace IxSApp.Data
{
    /// <summary>
    /// 
    /// </summary>
    public enum QueryStateType : int
    {
        /// <summary>
        /// The success
        /// </summary>
        Success = 0,
        /// <summary>
        /// The warning
        /// </summary>
        Warning,
        /// <summary>
        /// The error
        /// </summary>
        Error,
        /// <summary>
        /// The none
        /// </summary>
        None
    }
}
