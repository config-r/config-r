// <copyright file="Program.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.WindowsService
{
    using System;
    using Common.Logging;
    using ConfigR;
    using Topshelf;

    public static class Program
    {
        public static void Main(string[] args)
        {
            var log = LogManager.GetCurrentClassLogger();
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => log.Fatal((Exception)e.ExceptionObject);
            HostFactory.Run(x => x.Service<string>(o =>
            {
                o.ConstructUsing(n => n);
                o.WhenStarted(n => log.Info(Configurator.Get<string>("greeting")));
                o.WhenStopped(n => log.Info(Configurator.Get<string>("valediction")));
            }));
        }
    }
}
