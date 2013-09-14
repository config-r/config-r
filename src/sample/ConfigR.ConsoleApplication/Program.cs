// <copyright file="Program.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.ConsoleApplication
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using Common.Logging;
    using Common.Logging.Simple;
    using ConfigR;
    using ServiceStack.Text;

    public static class Program
    {
        public static void Main(string[] args)
        {
            LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter(LogLevel.Debug, false, true, true, null);

            // you can retreive settings as their underlying type
            var count = Configurator.Get<int>("Count");
            var uri = Configurator.Get<Uri>("Uri");

            Console.WriteLine("Count: {0}", count);
            Console.WriteLine("Uri: {0}", uri);

            // you can also retreive settings as dynamics
            var dynamicCount = Configurator.Get("Count");
            var dynamicUri = Configurator.Get("Uri");

            Console.WriteLine("Count: ({0}) {1}", dynamicCount.GetType(), dynamicCount);
            Console.WriteLine("Uri: ({0}) {1}", dynamicUri.GetType(), dynamicUri);

            // reset to original state for the samples below
            Configurator.Unload();

            // you can also have cascading configuration using multiple files
            Configurator.LoadLocal().Load("Custom1.csx").Load("Custom2.csx"); // Custom2.csx uses #load to get its data and does a nested load on Custom3.csx!
            count = Configurator.Get<int>("Count");
            uri = Configurator.Get<Uri>("Uri");
            var fromCustom1File = Configurator.Get<bool>("FromCustom1File");
            var fromCustom2File = Configurator.Get<bool>("FromCustom2File");
            var fromCustom3File = Configurator.Get<bool>("FromCustom3File");

            Console.WriteLine("Count: {0}", count);                     // this still comes from the first file (local "ConfigR.Sample.exe.csx")
            Console.WriteLine("Uri: {0}", uri);                         // this still comes from the first file (local "ConfigR.Sample.exe.csx")
            Console.WriteLine("FromCustom1File: {0}", fromCustom1File); // this comes from the second file ("Custom1.csx")
            Console.WriteLine("FromCustom2File: {0}", fromCustom2File); // this comes from the third file ("Custom2.csx"), defined by "Custom2.Data.csx"
            Console.WriteLine("FromCustom3File: {0}", fromCustom3File); // this comes from the fourth file ("Custom3.csx")

            // you can even use config located on the web!
            Configurator.Load(new Uri("https://gist.github.com/adamralph/6040898/raw/758951f2045cbf064f63a01c58e874e0f4d1a22a/sample-config.csx"));
            Console.WriteLine("web-greeting: {0}", Configurator.Get<string>("web-greeting"));

            // for completeness you can also use a file URI (or an FTP URI although that's not easily demonstrable)
            Configurator.Load(new Uri(Path.GetFullPath("Custom1.csx")));
            Console.WriteLine("FromCustom1File: {0}", Configurator.Get<bool>("FromCustom1File"));

            // reset to original state for the samples below
            Configurator.Unload();

            // your configuration script can also use types declared in your application
            Configurator.Load("Custom4.csx");
            Console.WriteLine("Foo: {0}", Configurator.Get<Foo>().ToJsv());

            Console.WriteLine("Brutalize a key with your favourite finger to exit.");
            Console.ReadKey();
        }
    }
}
