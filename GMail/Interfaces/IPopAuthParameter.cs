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
