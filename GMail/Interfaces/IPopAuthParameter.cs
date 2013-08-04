using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace As
{
    /// <summary>
    /// POP3 Authentication Parameter
    /// </summary>
    public interface IPopAuthParameter
        : IPopParameter
    {
        /// <summary>
        /// Gets the Token-Type for type of sending messages to a POP3 - Account
        /// </summary>
        PopTokenType Type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets all necessary authentication parameter
        /// </summary>
        /// <returns>Sequence with waiter</returns>
        IEnumerable<string> GetAuthParameter();
    }

}
