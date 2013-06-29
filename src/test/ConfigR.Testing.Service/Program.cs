// <copyright file="Program.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Testing.Service
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using Common.Logging;
    using Common.Logging.Log4Net;
    using ConfigR;
    using Topshelf;

    public static class Program
    {
        public static void Main(string[] args)
        {
            ConfigureLogging();

            var greeting = Configurator.Get<string>("greeting");
            var valediction = Configurator.Get<string>("valediction");
            HostFactory.Run(x => x.Service<TestService>(() => new TestService(greeting, valediction)));
        }

        private static void ConfigureLogging()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                var ex = (Exception)e.ExceptionObject;
                Common.Logging.LogManager.GetCurrentClassLogger().Fatal(ex);
                throw ex;
            };

            LogManager.Adapter = new Log4NetLoggerFactoryAdapter(new NameValueCollection
            {
                { "configType", "FILE" },
                { "configFile", Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), "log4net.config") },
            });
        }

        public class TestService : ServiceControl
        {
            private static readonly ILog log = Common.Logging.LogManager.GetCurrentClassLogger();

            private readonly string greeting;
            private readonly string valediction;

            public TestService(string greeting, string valediction)
            {
                this.greeting = greeting;
                this.valediction = valediction;
            }

            public bool Start(HostControl hostControl)
            {
                log.Info(this.greeting);
                return true;
            }

            public bool Stop(HostControl hostControl)
            {
                log.Info(this.valediction);
                return true;
            }
        }
    }
}
