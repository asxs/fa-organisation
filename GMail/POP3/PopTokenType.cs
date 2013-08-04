using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace As
{
    /// <summary>
    /// Gets the Token-Type for type of sending messages to a POP3 - Account
    /// </summary>
    public enum PopTokenType : int
    {
        /// <summary>
        /// Authorization
        /// </summary>
        Authorization = 0,

        /// <summary>
        /// Transaction
        /// </summary>
        Transaction = 1,

        /// <summary>
        /// Default
        /// </summary>
        None
    }
}
