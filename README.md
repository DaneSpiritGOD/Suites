# 介绍
Suites是本人在进行客户端开发时，对项目进行.net core/standard改造的过程中积累的一些类库。

[通用主机](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-2.1)是asp.net core中比较新的概念。
	```
	通用主机的目标是将 HTTP 管道从 Web 主机 API 中分离出来，从而启用更多的主机方案。 基于通用主机的消息、后台任务和其他非 HTTP 工作负载可从横切功能（如配置、依赖关系注入 [DI] 和日志记录）中受益。
	```

最主要的几个类库是`Suites.Core`、`Suites.Wpf.Core`和`Suites.Prism.Wpf.Fx`。

## Suites.Core
这个模块包含了一些常用的依赖包，比如：`Microsoft.AspNetCore`,`Microsoft.Extensions.Hosting`,`Microsoft.Extensions.Options.ConfigurationExtensions`,`NLog.Extensions.Logging`等。
我模仿Asp.Net Core中的`CreateDefaultBuilder`创建了通用主机环境下的类似功能：
```
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
```

## Suites.Wpf.Core
.net core SDK提供的通用主机是在console环境下的，而我长期从事WPF的客户端开发，自然需要WPF的HostLifetime，所以在阅读了微软的相关代码后，自己定义了`WpfLifetime`类。

## Suites.Prism.Wpf.Fx
与Wpf打交道自然免不了mvvm的框架，我一直使用的是Prism，所以仿造Prism.Unity.*，利用IHostBuilder编写了许多帮助类，从此在Wpf和Prism的环境下，自由地使用微软提供的依赖注入。


## 使用方法
见`Suites.Demo`