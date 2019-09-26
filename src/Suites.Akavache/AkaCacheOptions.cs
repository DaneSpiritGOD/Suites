using System;
using System.Collections.Generic;
using System.Text;

namespace Suites.Akavache
{
    public class AkaCacheOptions
    {
        public AkaCacheOptions(string name, string path)
        {
            Name = StringNullOrWhiteSpaceException.Assert(name, nameof(name));
            PersistenceMediaDirPath = StringNullOrWhiteSpaceException.Assert(path, nameof(path));
        }

        public string Name { get; }
        public string PersistenceMediaDirPath { get; set; }
        public bool NeedEncryption { get; set; }
    }
}