// <copyright file="Program.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Tests.Smoke.Service
{
    using System;
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
            MainAsync().GetAwaiter().GetResult();
        }

        public static async Task MainAsync()
        {
            var config = await new Config().UseRoslynCSharpLoader().Load();

            using (var target = new ColoredConsoleTarget())
            {
                var loggingConfig = new LoggingConfiguration();
                loggingConfig.AddTarget("console", target);
                loggingConfig.LoggingRules.Add(new LoggingRule("*", NLog.LogLevel.Trace, target));
                LogManager.Configuration = loggingConfig;

                var log = LogProvider.GetCurrentClassLogger();

                AppDomain.CurrentDomain.UnhandledException += (sender, e) => log.FatalException("Unhandled exception.", (Exception)e.ExceptionObject);
                HostFactory.Run(x => x.Service<string>(o =>
                {
                    o.ConstructUsing(n => n);
                    o.WhenStarted(n => log.Info((string)config.Settings<Settings>().Greeting));
                    o.WhenStopped(n => log.Info((string)config.Settings<Settings>().Valediction));
                }));
            }
        }
    }
}
