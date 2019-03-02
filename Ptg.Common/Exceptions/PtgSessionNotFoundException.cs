using System;

namespace Ptg.Common.Exceptions
{
    public class PtgNotFoundException : ApplicationException
    {
        public PtgNotFoundException(string message) : base(message) { }
    }
}
