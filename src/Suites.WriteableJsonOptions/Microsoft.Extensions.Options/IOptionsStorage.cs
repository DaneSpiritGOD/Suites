using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Options.Writable
{
    public interface IOptionsStorage
    {
        void Restore();
        void Save();
    }
}
