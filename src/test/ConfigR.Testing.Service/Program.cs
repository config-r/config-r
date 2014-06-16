// <copyright file="Program.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Testing.Service
{
    using System;
    using Common.Logging;
    using ConfigR;
    using Topshelf;

    public static class Program
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();

        public static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => log.Fatal((Exception)e.ExceptionObject);
            HostFactory.Run(x => x.Service<string>(o =>
            {
                o.ConstructUsing(n => n);
                o.WhenStarted(n => log.Info(Config.Global.Get<Settings>("settings").Greeting));
                o.WhenStopped(n => log.Info(Config.Global.Get<Settings>("settings").Valediction));
            }));
        }
    }
}
