using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Threading;
using Microsoft.Extensions.Logging;
using System.Windows;

namespace Microsoft.Extensions.Hosting
{
    public class WpfLifetime : IHostLifetime, IDisposable
    {
        private readonly ManualResetEvent _shutdownBlock = new ManualResetEvent(false);

        private WpfLifetimeOptions Options { get; }

        private IHostingEnvironment Environment { get; }

        private IApplicationLifetime ApplicationLifetime { get; }

        private readonly ILogger<WpfLifetime> _logger;
        private readonly Application _app;

        public WpfLifetime(
            Application app,
            IOptions<WpfLifetimeOptions> options,
            IHostingEnvironment environment,
            IApplicationLifetime applicationLifetime,
            ILogger<WpfLifetime> logger)
        {
            Options = NamedNullException.Assert(options?.Value, nameof(options));
            Environment = NamedNullException.Assert(environment, nameof(environment));
            ApplicationLifetime = NamedNullException.Assert(applicationLifetime, nameof(applicationLifetime));
            _logger = NamedNullException.Assert(logger, nameof(logger));
            _app = NamedNullException.Assert(app, nameof(app));
        }

        public Task WaitForStartAsync(CancellationToken cancellationToken)
        {
            if (!Options.SuppressStatusMessages)
            {
                ApplicationLifetime.ApplicationStarted.Register(delegate
                {
                    _logger.LogInformation("Application started.");
                    _logger.LogInformation($"Hosting environment: {Environment.EnvironmentName}");
                    _logger.LogInformation($"Content root path: {Environment.ContentRootPath}");
                });
            }
            AppDomain.CurrentDomain.ProcessExit += delegate
            {
                ApplicationLifetime.StopApplication();
                _shutdownBlock.WaitOne();
            };
            _app.Exit += delegate (object sender, ExitEventArgs e)
            {
                ApplicationLifetime.StopApplication();
            };
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _shutdownBlock.Set();
        }
    }

}
