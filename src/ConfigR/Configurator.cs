// <copyright file="Configurator.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System.Collections.Generic;

    public static class Configurator
    {
        private static IConfigurator current = new LocalConfigurator();

        public static IConfigurator Current
        {
            get { return current; }
        }

        public static IDictionary<string, dynamic> Configuration
        {
            get { return current.Configuration; }
        }

        public static void Add(string key, dynamic value)
        {
            current.Add(key, value);
        }

        public static void Load()
        {
            current.Load();
        }

        public static dynamic Get(string key)
        {
            return current[key];
        }

        public static dynamic GetOrDefault(string key)
        {
            return current.GetOrDefault(key);
        }

        public static T Get<T>(string key)
        {
            return current.Get<T>(key);
        }

        public static T GetOrDefault<T>(string key)
        {
            return current.GetOrDefault<T>(key);
        }
    }
}
