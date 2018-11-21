[![Build Status](https://dev.azure.com/DaneSpiritGOD/Suites/_apis/build/status/DaneSpiritGOD.Suites)](https://dev.azure.com/DaneSpiritGOD/Suites/_build/latest?definitionId=1)

# ����
Suites�Ǳ����ڽ��пͻ��˿���ʱ������Ŀ����.net core/standard����Ĺ����л��۵�һЩ��⡣

[ͨ������](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-2.1)��asp.net core�бȽ��µĸ��
	```
	ͨ��������Ŀ���ǽ� HTTP �ܵ��� Web ���� API �з���������Ӷ����ø�������������� ����ͨ����������Ϣ����̨����������� HTTP �������ؿɴӺ��й��ܣ������á�������ϵע�� [DI] ����־��¼�������档
	```

����Ҫ�ļ��������`Suites.Core`��`Suites.Wpf.Core`��`Suites.Prism.Wpf.Fx`��

## Suites.Core
���ģ�������һЩ���õ������������磺`Microsoft.AspNetCore`,`Microsoft.Extensions.Hosting`,`Microsoft.Extensions.Options.ConfigurationExtensions`,`NLog.Extensions.Logging`�ȡ�
��ģ��Asp.Net Core�е�`CreateDefaultBuilder`������ͨ�����������µ����ƹ��ܣ�
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
.net core SDK�ṩ��ͨ����������console�����µģ����ҳ��ڴ���WPF�Ŀͻ��˿�������Ȼ��ҪWPF��HostLifetime���������Ķ���΢�����ش�����Լ�������`WpfLifetime`�ࡣ

## Suites.Prism.Wpf.Fx
��Wpf�򽻵���Ȼ�ⲻ��mvvm�Ŀ�ܣ���һֱʹ�õ���Prism�����Է���Prism.Unity.*������IHostBuilder��д���������࣬�Ӵ���Wpf��Prism�Ļ����£����ɵ�ʹ��΢���ṩ������ע�롣


## ʹ�÷���
��`Suites.Demo`