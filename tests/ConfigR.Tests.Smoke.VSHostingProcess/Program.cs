// <copyright file="Program.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Tests.Smoke.VSHostingProcess
{
    using System;
    using System.Threading.Tasks;
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
                var loggingConfig = new LoggingConfiguration();
                loggingConfig.AddTarget("console", target);
                loggingConfig.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, target));
                LogManager.Configuration = loggingConfig;

                MainAsync().GetAwaiter().GetResult();
            }
        }

        public static async Task MainAsync()
        {
            Console.WriteLine((await new Config().UseRoslynCSharpLoader().LoadDynamic()).Greeting<string>());

            Console.WriteLine("Brutalize a key with your favourite finger to exit.");
            Console.ReadKey();
        }
    }
}
