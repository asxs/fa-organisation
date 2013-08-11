using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace As
{
    public class FaOrganisationUpdate
        : FaOrganisationAbstract, IDisposable
    {
        public FaOrganisationUpdate()
            : base()
        {

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
