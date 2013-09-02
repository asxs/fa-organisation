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
using System.Data;
using System.Data.Common;

namespace IxSApp
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlServerTransactionSystem
        : IxSAppTransactionSystem
    {
        /// <summary>
        /// The transaction string
        /// </summary>
        protected string transactionString = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerTransactionSystem"/> class.
        /// </summary>
        public SqlServerTransactionSystem()
        {

        }

        #region SqlServerTransactionSystem (IxSAppTransactionSystem)

        /// <summary>
        /// Sets the isolation level.
        /// </summary>
        /// <param name="isolationLevel">The isolation level.</param>
        public void SetIsolationLevel(IsolationLevel isolationLevel)
        {
            Isolation = isolationLevel;
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            transactionString = string.Empty;
        }

        /// <summary>
        /// Catches the transaction keyword.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public TransactionSystemDirectionType CatchTransactionKeyword(string item)
        {
            var direction
                = TransactionSystemDirectionType.Error;

            if (Isolation != IsolationLevel.Chaos && Isolation != IsolationLevel.Unspecified)
            {
                switch (Isolation)
                {
                    case IsolationLevel.ReadCommitted:
                        {
                            item = item.ToUpper();
                            switch (item)
                            {
                                case "BEGIN":
                                    direction = TransactionSystemDirectionType.Forward;
                                    item =
                                        string.Concat(item, " "); //clean it
                                    break;
                                case "TRANSACTION":
                                    direction = TransactionSystemDirectionType.Forward;
                                    item =
                                        string.Concat(item, " ");
                                    break;
                                case "SET":
                                    direction = TransactionSystemDirectionType.Forward;
                                    item =
                                        string.Concat(item, " ");
                                    break;
                                case "ISOLATION":
                                    direction = TransactionSystemDirectionType.Forward;
                                    item =
                                        string.Concat(item, " ");
                                    break;
                                case "LEVEL":
                                    direction = TransactionSystemDirectionType.Forward;
                                    item =
                                        string.Concat(item, " ");
                                    break;
                            }
                        }
                        break;
                }
            }

            switch (Isolation)
            {
                case IsolationLevel.ReadCommitted:
                    {
                        item = item.ToUpper();
                        switch (item)
                        {
                            case "READ":
                                direction = TransactionSystemDirectionType.Forward;
                                item =
                                    string.Concat(item, " ");
                                break;
                            case "COMMITTED":
                                direction = TransactionSystemDirectionType.Forward;
                                item =
                                    string.Concat(item, " ");
                                break;
                        }
                    }
                    break;
            }

            if (direction ==
                TransactionSystemDirectionType.Forward)
                transactionString = string.Concat(transactionString, item);

            return direction;
        }

        /// <summary>
        /// Verifies the keywords.
        /// </summary>
        public void VerifyKeywords() //regexp
        {

        }

        /// <summary>
        /// Generates the command text.
        /// </summary>
        /// <returns></returns>
        public string GenerateCommandText()
        {
            return string.Empty;
        }

        /// <summary>
        /// Gets the isolation.
        /// </summary>
        /// <value>
        /// The isolation.
        /// </value>
        public IsolationLevel Isolation { get; internal set; }
        /// <summary>
        /// Gets the system.
        /// </summary>
        /// <value>
        /// The system.
        /// </value>
        public SqlSystem System { get; internal set; }
        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public TransactionLayerProperties Properties { get; internal set; }
        /// <summary>
        /// Gets the keywords.
        /// </summary>
        /// <value>
        /// The keywords.
        /// </value>
        public string[] Keywords { get; internal set; }

        #endregion
    }
}
