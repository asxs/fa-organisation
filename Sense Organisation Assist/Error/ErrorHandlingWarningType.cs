﻿/* 
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
    public enum ErrorHandlingWarningType : int
    {
        /// <summary>
        /// The usual
        /// </summary>
        Usual = 0,
        /// <summary>
        /// The warning
        /// </summary>
        Warning = 1,
        /// <summary>
        /// The critical
        /// </summary>
        Critical = 2,
        /// <summary>
        /// The error
        /// </summary>
        Error = 3,
        /// <summary>
        /// The none
        /// </summary>
        None
    }
}
