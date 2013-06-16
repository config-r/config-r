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
            var count = Configurator.Get<int>("Count");
            var uri = Configurator.Get<Uri>("Uri");

            Console.WriteLine("Count: ({0}) {1}", count.GetType(), count);
            Console.WriteLine("Uri: ({0}) {1}", uri.GetType(), uri);

            Console.WriteLine("Brutalize a key with your favourite finger to exit.");
            Console.ReadKey();
        }
    }
}
