using System;
using System.Collections.Generic;
using System.Text;
using static Suites.Akavache.Properties.Resources;

namespace Suites.Akavache
{
    public class CreateSameNameMediaException : Exception
    {
        public CreateSameNameMediaException(string name)
            : base(string.Format(CreateSameNameMediaExceptionString, name)) { }
    }
}
