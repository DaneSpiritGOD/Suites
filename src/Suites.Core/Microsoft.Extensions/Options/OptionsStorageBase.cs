using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Options.Writable
{
    public abstract class OptionsStorageBase<TOptions, TModel> : IOptionsStorage, IDisposable
        where TOptions : class, new()
        where TModel : class, INotifyPropertyChanged
    {
        protected readonly IWritableOptions<TOptions> _options;
        protected readonly TModel _model;

        public OptionsStorageBase(IWritableOptions<TOptions> options,
            TModel model)
        {
            _options = NamedNullException.Assert(options, nameof(options));
            _model = NamedNullException.Assert(model, nameof(model));
            _model.PropertyChanged += OnModelPropertyChanged;
        }

        protected abstract void OnModelPropertyChanged(object sender,
            PropertyChangedEventArgs e);

        public abstract void Restore();
        public abstract void Save();

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls        

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _model.PropertyChanged -= OnModelPropertyChanged;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~OptionsStorageBase() {
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
