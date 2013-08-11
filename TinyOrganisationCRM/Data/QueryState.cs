using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace As.Data
{
    public enum QueryStateType : int
    {
        Success = 0,
        Warning,
        Error,
        None
    }
}
