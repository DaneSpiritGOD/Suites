using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Suites.Core.Process
{
    public sealed class Singleton : IDisposable
    {
        private readonly string _mutexName;
        private Mutex _systemMutex;
        private Singleton(string mutexName)
        {
            _mutexName = StringNullOrEmptyException.Assert(mutexName, nameof(mutexName));
            _systemMutex = default;
        }

        public static Singleton Create(string mutexName)
        {
            return new Singleton(mutexName);
        }

        public bool IsOwnedByCurrentProcess
        {
            get
            {
                if (_systemMutex != default)
                {
                    return true;
                }

                var mutex = new Mutex(true, _mutexName, out var createNew);
                if (createNew)
                {
                    _systemMutex = mutex;
                    return true;
                }
                else
                {
                    _systemMutex = default;
                    return false;
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _systemMutex?.Dispose();
                    _systemMutex = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Singleton() {
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
