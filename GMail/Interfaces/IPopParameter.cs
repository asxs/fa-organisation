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

namespace IxSApp
{

    /// <summary>
    /// POP3 Parameter with Tokens for Authentication or Transaction (for e.g. USER or PASS or LIST)
    /// </summary>
    public interface IPopParameter
    {
        /// <summary>
        /// Gets or sets the POP3 Token
        /// </summary>
        string Token { get; set; }
    }
}
