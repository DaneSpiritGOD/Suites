using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Hosting
{
    public class ProcessNotSingletonException : RuntimeException
    {
        public ProcessNotSingletonException() : base("second app instance created!") { }

        public ProcessNotSingletonException(string msg) : base(msg)
        {
        }
    }
}
