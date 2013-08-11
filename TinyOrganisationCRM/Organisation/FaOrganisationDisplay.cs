using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace As
{
    public class FaOrganisationDisplay
        : FaOrganisationAbstract, IDisposable
    {
        public FaOrganisationDisplay()
            : base()
        {

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
