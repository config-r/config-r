// <copyright file="Program.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Sample
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using ConfigR;

    public static class Program
    {
        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String,System.Object,System.Object)", Justification = "Just a sample.")]
        public static void Main(string[] args)
        {
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
            Configurator.LoadLocal().Load("Custom1.csx").Load("Custom2.csx");
            count = Configurator.Get<int>("Count");
            uri = Configurator.Get<Uri>("Uri");
            var fromCustom1File = Configurator.Get<bool>("FromCustom1File");
            var fromCustom2File = Configurator.Get<bool>("FromCustom2File");
            
            Console.WriteLine("Count: {0}", count);                     // this still comes from the first file (local "ConfigR.Sample.exe.csx")
            Console.WriteLine("Uri: {0}", uri);                         // this still comes from the first file (local "ConfigR.Sample.exe.csx")
            Console.WriteLine("FromCustom1File: {0}", fromCustom1File); // this comes from the second file ("Custom1.csx")
            Console.WriteLine("FromCustom2File: {0}", fromCustom2File); // this comes from the third file ("Custom2.csx")

            Console.WriteLine("Brutalize a key with your favourite finger to exit.");
            Console.ReadKey();
        }
    }
}
