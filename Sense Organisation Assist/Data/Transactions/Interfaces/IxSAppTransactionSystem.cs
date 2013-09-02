using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IxSApp
{
    /// <summary>
    /// 
    /// </summary>
    public interface IxSAppTransactionSystem
    {
        /// <summary>
        /// Sets the isolation level.
        /// </summary>
        /// <param name="isolationLevel">The isolation level.</param>
        void SetIsolationLevel(IsolationLevel isolationLevel);
        /// <summary>
        /// Resets this instance.
        /// </summary>
        void Reset();
        /// <summary>
        /// Catches the transaction keyword.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        TransactionSystemDirectionType CatchTransactionKeyword(string item);
        /// <summary>
        /// Verifies the keywords.
        /// </summary>
        void VerifyKeywords(); //regexp
        /// <summary>
        /// Generates the command text.
        /// </summary>
        /// <returns></returns>
        string GenerateCommandText();
        /// <summary>
        /// Gets the isolation.
        /// </summary>
        /// <value>
        /// The isolation.
        /// </value>
        IsolationLevel Isolation { get; }
        /// <summary>
        /// Gets the system.
        /// </summary>
        /// <value>
        /// The system.
        /// </value>
        SqlSystem System { get; }
        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        TransactionLayerProperties Properties { get; }
        /// <summary>
        /// Gets the keywords.
        /// </summary>
        /// <value>
        /// The keywords.
        /// </value>
        string[] Keywords { get; }
    }
}
