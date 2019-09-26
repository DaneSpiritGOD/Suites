using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Akavache.Sqlite3;
using System.Collections.Concurrent;
using Akavache;

namespace Suites.Akavache
{
    internal class DefaultAkavacheManager : IAkavacheManager, IDisposable
    {
        public const string DefaultMediaName = nameof(DefaultMediaName);
        private readonly ConcurrentDictionary<string, IBlobCache> _caches;
        public DefaultAkavacheManager()
        {
            _caches = new ConcurrentDictionary<string, IBlobCache>(StringComparer.OrdinalIgnoreCase);
        }

        public void AddPersistenceMedia(AkaCacheOptions options)
        {
            NamedNullException.Assert(options, nameof(options));

            var name = StringNullOrWhiteSpaceException.Assert(options.Name, nameof(options.Name));
            if (_caches.ContainsKey(name))
            {
                throw new CreateSameNameMediaException(name);
            }

            var path = Path.GetFullPath(options.PersistenceMediaDirPath);
            path.EnsureDirExists();

            var full = Path.Combine(path, $"{name}.db");
            _caches[name] = options.NeedEncryption ? new SQLiteEncryptedBlobCache(full) : new SqlRawPersistentBlobCache(full);
        }

        public IBlobCache GetMedia(string name)
        {
            NamedNullException.Assert(name, nameof(name));

            if (!_caches.ContainsKey(name))
            {
                throw new DoNotExistSuchNameMediaException(name);
            }

            return _caches[name];
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    foreach (var pair in _caches)
                    {
                        pair.Value?.Dispose();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DefaultAkavacheManager()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
