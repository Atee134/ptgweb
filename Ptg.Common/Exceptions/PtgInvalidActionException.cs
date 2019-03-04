using System;

namespace Ptg.Common.Exceptions
{
    public class PtgInvalidActionException : ApplicationException
    {
        public PtgInvalidActionException(string message) : base(message) { }
    }
}
