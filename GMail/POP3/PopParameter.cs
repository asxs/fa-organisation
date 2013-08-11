/* --- Builded by Lars Ulrich Herrmann (c) 2013 with f.fN. Sense Applications in year August 2013
 * The code can be used in other Applications if it makes sense
 * If it makes sense the code can be used in this Application
 * I hold the rights on all lines of code and if it makes sense you can contact me over the publish site
 
 * Feel frXsApp to leave a comment
 * --- End
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IxSApp
{
    /// <summary>
    /// Explicit use of Authentication or Transaction parameters for interfaces of IPopAuthParameter or IPopTransactionParameter
    /// </summary>
    public sealed class PopParameter
        :
        IPopAuthParameter,
        IPopTransactionParameter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token">Can be a custom token from the protocol type like USER or PASS or LIST</param>
        public PopParameter(string token)
        {
            Token = token;
        }

        /// <summary>
        /// Gets or sets the token for communication with the POP3 Server
        /// </summary>
        public string Token
        {
            get;
            set;
        }

        #region PopParameter (Authentication)

        /// <summary>
        /// Gets the Token-Type for type of sending messages to a POP3 - Account
        /// </summary>
        PopTokenType IPopAuthParameter.Type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets all necessary authentication parameter
        /// </summary>
        /// <returns>Sequence with waiter</returns>
        IEnumerable<string> IPopAuthParameter.GetAuthParameter()
        {
            foreach (var parameter in new string[] { Constants.UserAuthenticationString, Constants.PasswordAuthenticationString })
                yield return parameter;

            yield break;
        }

        #endregion

        #region PopParameter (Transaction)

        /// <summary>
        /// Gets the Token-Type for type of sending messages to a POP3 - Account
        /// </summary>
        PopTokenType IPopTransactionParameter.Type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets all necessary transaction parameter
        /// </summary>
        /// <returns>Sequence with waiter</returns>
        IEnumerable<string> IPopTransactionParameter.GetTransactionParameter()
        {
            foreach (var parameter in new string[] { Constants.ListTokenString })
                yield return parameter;

            yield break;
        }

        #endregion
    }

}
