// <copyright file="Program.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Tests.Smoke.VSHostingProcess
{
    using System;
    using ConfigR;
    using NLog;
    using NLog.Config;
    using NLog.Targets;

    public static class Program
    {
        public static void Main()
        {
            using (var target = new ColoredConsoleTarget())
            {
                var config = new LoggingConfiguration();
                config.AddTarget("console", target);
                config.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, target));
                LogManager.Configuration = config;

                Console.WriteLine(Config.Global.Get<string>("greeting"));

                Console.WriteLine("Brutalize a key with your favourite finger to exit.");
                Console.ReadKey();
            }
        }
    }
}
