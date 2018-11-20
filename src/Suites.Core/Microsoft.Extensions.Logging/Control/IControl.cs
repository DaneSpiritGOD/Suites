using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Logging.Control
{
    public interface IControl
    {
        void Append(string text);
        void Clear();
    }
}
