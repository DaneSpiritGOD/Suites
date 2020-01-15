using System;

namespace Microsoft.Extensions.Logging
{
    internal class NullExternalScopeProvider : IExternalScopeProvider
    {
        public static IExternalScopeProvider Instance { get; } = new NullExternalScopeProvider();

        private NullExternalScopeProvider()
        {
        }

        void IExternalScopeProvider.ForEachScope<TState>(Action<object, TState> callback, TState state)
        {
        }

        IDisposable IExternalScopeProvider.Push(object state) => NullScope.Instance;
    }

    internal class NullScope : IDisposable
    {
        public static NullScope Instance { get; } = new NullScope();

        private NullScope()
        {
        }

        public void Dispose()
        {
        }
    }
}
