// <copyright file="Program.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Tests.Smoke.Service
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using ConfigR;
    using ConfigR.Tests.Smoke.Service.Logging;
    using NLog;
    using NLog.Config;
    using NLog.Targets;
    using Topshelf;

    public static class Program
    {
        public static void Main()
        {
            using (var target = new ColoredConsoleTarget())
            {
                var loggingConfig = new LoggingConfiguration();
                loggingConfig.AddTarget("console", target);
                loggingConfig.LoggingRules.Add(new LoggingRule("*", NLog.LogLevel.Trace, target));
                LogManager.Configuration = loggingConfig;

                MainAsync(LogProvider.GetCurrentClassLogger()).GetAwaiter().GetResult();
            }
        }

        private static async Task MainAsync(ILog log)
        {
            var settings = await new Config()
                .UseRoslynCSharpLoader()
                .UseRoslynCSharpLoader("https://gist.githubusercontent.com/adamralph/9c4d6a6a705e1762646fbcf124f634f9/raw/7817c0282d334512b9b554ba59c293a24b3c21fd/sample-config4.csx")
                .UseRoslynCSharpLoader(new Uri(Path.GetFullPath("Test1.csx")).ToString())
                .UseRoslynCSharpLoader(new Uri(Path.GetFullPath("Test2.csx")).ToString())
                .Load<Settings>();

            AppDomain.CurrentDomain.UnhandledException += (sender, e) => log.FatalException("Unhandled exception.", (Exception)e.ExceptionObject);
            HostFactory.Run(x => x.Service<string>(o =>
            {
                o.ConstructUsing(n => n);
                o.WhenStarted(n =>
                {
                    log.Info(settings.Greeting);
                    log.Info(settings.WebGreeting);
                    log.Info(settings.Foo);
                    log.Info(settings.Bar);
                    log.Info(settings.Baz);
                });

                o.WhenStopped(n =>
                {
                    log.Info(settings.Valediction);
                    log.Info(settings.WebValediction);
                });
            }));
        }
    }
}
