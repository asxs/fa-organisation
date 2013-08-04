using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace As
{
    public class FaOrganisationRemove
        : FaOrganisationAbstract, IDisposable
    {
        public FaOrganisationRemove()
            : base()
        {

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
