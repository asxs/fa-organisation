using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace As
{
    public static class ErrorHandler
    {
        public static void Write(ErrorHandlingWarningType typeOfWarning)
        {
            switch (typeOfWarning)
            {
                case ErrorHandlingWarningType.Usual:
                    break;
                case ErrorHandlingWarningType.Warning:
                    break;
                case ErrorHandlingWarningType.Critical:
                    break;
                case ErrorHandlingWarningType.Error:
                    break;
            }
        }
    }
}
