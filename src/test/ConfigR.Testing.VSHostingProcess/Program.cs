// <copyright file="Program.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Testing.VSHostingProcess
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
            var config = new LoggingConfiguration();
            var target = new ColoredConsoleTarget();
            config.AddTarget("console", target);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, target));
            LogManager.Configuration = config;

            Console.WriteLine(Config.Global.Get<string>("greeting"));

            Console.WriteLine("Brutalize a key with your favourite finger to exit.");
            Console.ReadKey();
        }
    }
}
