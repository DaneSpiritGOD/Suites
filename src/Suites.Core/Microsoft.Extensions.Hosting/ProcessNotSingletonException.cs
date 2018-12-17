using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Hosting
{
    public class ProcessNotSingletonException : RuntimeException
    {
        public ProcessNotSingletonException() : base("不允许存在相同的应用程序实例!") { }

        public ProcessNotSingletonException(string msg) : base(msg)
        {
        }
    }
}
