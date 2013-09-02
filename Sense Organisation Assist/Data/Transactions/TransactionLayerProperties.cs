using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IxSApp
{
    /// <summary>
    /// 
    /// </summary>
    public struct TransactionLayerProperties
    {
        /// <summary>
        /// Gets the command text.
        /// </summary>
        /// <value>
        /// The command text.
        /// </value>
        public string CommandText
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets a value indicating whether this instance has errors.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has errors; otherwise, <c>false</c>.
        /// </value>
        public bool HasErrors
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <value>
        /// The command.
        /// </value>
        public IDbCommand Command { get; internal set; }
    }
}
