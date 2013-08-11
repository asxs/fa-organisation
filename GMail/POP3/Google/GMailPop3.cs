/* --- Builded by Lars Ulrich Herrmann (c) 2013 with f.fN. Sense Applications in year August 2013
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
    /// Google GMail Property Package
    /// </summary>
    public static class GMailPop3
    {
        /// <summary>
        /// Gets the POP3 Server from GMail
        /// </summary>
        public const string Server = "pop.gmail.com";

        /// <summary>
        /// POP3 (SSL)
        /// </summary>
        public const uint SslPort = 995;

        /// <summary>
        /// Gets or sets the client IP Address (for e.g. 192.168.xxx.xxx)
        /// </summary>
        public static string Client { get; set; }
    }

}
