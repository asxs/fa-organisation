using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace As
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
