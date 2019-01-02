using Suites.Core.Process;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Hosting
{
    public class ProcessSingletonService : BackgroundService, IHostedService
    {
        public const string MutexNameKey = "MutexName";

        private readonly IConfiguration _config;
        private readonly IHostLifetime _hostLifetime;
        private Singleton _procSingleton;

        public ProcessSingletonService(
            IConfiguration config,
            IHostLifetime hostLifetime)
        {
            _config = NamedNullException.Assert(config, nameof(config));
            _hostLifetime = NamedNullException.Assert(hostLifetime, nameof(hostLifetime));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var mutexName = _config.GetValue(MutexNameKey, TimeStamp.DetailNow);
            _procSingleton = Singleton.Create(mutexName);
            if (!_procSingleton.IsOwnedByCurrentProcess)
            {
                return Task.FromException(new ProcessNotSingletonException());

                //(_hostLifetime as IDisposable).Dispose();
                //Environment.Exit(0);
            }
            return Task.CompletedTask;
        }
    }
}