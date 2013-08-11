using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace As
{
    public class FaOrganisationEdit
        : FaOrganisationAbstract, IDisposable
    {
        public FaOrganisationEdit()
            : base()
        {

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
