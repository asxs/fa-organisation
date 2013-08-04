using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace As
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
