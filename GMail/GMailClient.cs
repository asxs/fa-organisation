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
    /// Little GMail Client that receives only a List of E-Mails
    /// </summary>
    public class GMailClient
        : IDisposable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GMailClient()
        {

        }

        #region GMailClient (IDisposable)

        /// <summary>
        /// Disposing all save or unsaved components
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposing all save or unsaved components
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
        }

        #endregion
    }

}
