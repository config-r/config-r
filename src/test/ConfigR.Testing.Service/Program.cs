// <copyright file="Program.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Testing.Service
{
    using System;
    using ConfigR;
    using ConfigR.Testing.Service.Logging;
    using NLog;
    using NLog.Config;
    using NLog.Targets;
    using Topshelf;

    public static class Program
    {
        private static readonly ILog log = LogProvider.GetCurrentClassLogger();

        public static void Main()
        {
            using (var target = new ColoredConsoleTarget())
            {
                var config = new LoggingConfiguration();
                config.AddTarget("console", target);
                config.LoggingRules.Add(new LoggingRule("*", NLog.LogLevel.Trace, target));
                LogManager.Configuration = config;

                AppDomain.CurrentDomain.UnhandledException += (sender, e) => log.FatalException("Unhandled exception.", (Exception)e.ExceptionObject);
                HostFactory.Run(x => x.Service<string>(o =>
                {
                    o.ConstructUsing(n => n);
                    o.WhenStarted(n => log.Info(Config.Global.Get<Settings>("settings").Greeting));
                    o.WhenStopped(n => log.Info(Config.Global.Get<Settings>("settings").Valediction));
                }));
            }
        }
    }
}
