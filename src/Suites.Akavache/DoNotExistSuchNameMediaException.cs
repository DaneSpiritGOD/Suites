using System;
using System.Collections.Generic;
using System.Text;
using static Suites.Akavache.Properties.Resources;

namespace Suites.Akavache
{
    public class DoNotExistSuchNameMediaException : Exception
    {
        public DoNotExistSuchNameMediaException(string name) 
            : base(string.Format(DoNotExistSuchNameMediaExceptionString,name))
        {
        }
    }
}
