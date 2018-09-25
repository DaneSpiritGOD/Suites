using Microsoft.Extensions.Logging.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Logging.Control
{
    public class NullControl : IControl
    {
        public void Append(string text)
        {
        }

        public void Clear()
        {
        }
    }
}
