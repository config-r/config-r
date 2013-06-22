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
            // you can retreive settings as dynamics
            var dynamicCount = Configurator.Get("Count");
            var dynamicUri = Configurator.Get("Uri");

            Console.WriteLine("Count: ({0}) {1}", dynamicCount.GetType(), dynamicCount);
            Console.WriteLine("Uri: ({0}) {1}", dynamicUri.GetType(), dynamicUri);

            // you can also retreive settings as their underlying type
            var count = Configurator.Get<int>("Count");
            var uri = Configurator.Get<Uri>("Uri");

            Console.WriteLine("Count * 2: {0}", count * 2);
            Console.WriteLine("Uri.AbsolutePath: {0}", uri.AbsolutePath);

            Console.WriteLine("Brutalize a key with your favourite finger to exit.");
            Console.ReadKey();
        }
    }
}
