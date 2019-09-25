using System;
using System.Collections.Generic;
using System.Text;
using Akavache;

namespace Suites.Akavache
{
    public interface IAkavacheManager : IDisposable
    {
        void AddPersistenceMedia(AkaCacheOptions options);
        IBlobCache GetMedia(string name);
    }
}
