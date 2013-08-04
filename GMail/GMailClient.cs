using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace As
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
