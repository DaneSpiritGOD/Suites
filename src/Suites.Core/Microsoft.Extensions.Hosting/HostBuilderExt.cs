using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NLog.Extensions.Logging;

namespace Microsoft.Extensions.Hosting
{
    public static class HostBuilderExt
    {
        public static IHostBuilder CreateDefaultHostBuilder() => CreateDefaultHostBuilder(null);

        public static IHostBuilder CreateDefaultHostBuilder(string[] args)
        {
            return new HostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.AddJsonFile("hostsettings.json", true, true);
                    configHost.AddEnvironmentVariables(prefix: "PREFIX_");

                    if (args != null)
                    {
                        configHost.AddCommandLine(args);
                    }
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.AddJsonFile("appsettings.json", true, true);
                    configApp.AddJsonFile(
                        $"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
                        true, true);
                    configApp.AddEnvironmentVariables(prefix: "PREFIX_");

                    if (args != null)
                    {
                        configApp.AddCommandLine(args);
                    }
                })
                .AddNLogExt();
        }

        public static IHostBuilder AddNLogExt(this IHostBuilder builder)
        {
            return builder.ConfigureLogging((hostContext, configLogging) =>
             {
                 configLogging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
                 //configLogging.AddConsole();
                 //configLogging.AddDebug();
                 var services = configLogging.Services;
                 services.AddSingleton<ILoggerFactory, LoggerFactory>();
                 services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
                 configLogging.AddNLog(
                     new NLogProviderOptions
                     {
                         CaptureMessageTemplates = true,
                         CaptureMessageProperties = true
                     });
             });
        }

        public static IHostBuilder UseHostedService<THostedService>(this IHostBuilder builder)
            where THostedService : class, IHostedService
        {
            return builder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IHostedService, THostedService>();
            });
        }

        public static IHostBuilder ForceProcessSingleton(this IHostBuilder builder)
        {
            return builder.UseHostedService<ProcessSingletonService>();
        }
    }
}
