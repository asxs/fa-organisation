using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace As
{
    public class FaOrganisationAppend 
        : FaOrganisationAbstract, IDisposable
    {
        public FaOrganisationAppend()
        {

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
