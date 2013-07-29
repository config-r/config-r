// <copyright file="Configurator.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.Collections.Generic;

    public static class Configurator
    {
        private static CascadingConfigurator current = new CascadingConfigurator();

        public static IConfigurator Current
        {
            get { return current; }
        }

        public static IEnumerable<KeyValuePair<string, dynamic>> Items
        {
            get { return current.Items; }
        }

        public static IConfigurator Add(dynamic value)
        {
            return current.Add(value as object);
        }

        public static IConfigurator Add(string key, dynamic value)
        {
            return current.Add(key, value);
        }

        public static dynamic Get(string key)
        {
            return current[key];
        }

        public static dynamic GetOrDefault(string key)
        {
            return current.GetOrDefault(key);
        }

        public static T Get<T>()
        {
            return current.Get<T>();
        }

        public static T Get<T>(string key)
        {
            return current.Get<T>(key);
        }

        public static T GetOrDefault<T>(string key)
        {
            return current.GetOrDefault<T>(key);
        }

        public static CascadingConfigurator Load(Uri uri)
        {
            return current.Load(uri);
        }

        public static CascadingConfigurator Load(string path)
        {
            return current.Load(path);
        }

        public static CascadingConfigurator LoadLocal()
        {
            return current.LoadLocal();
        }

        public static CascadingConfigurator Load(IConfigurator configurator)
        {
            return current.Load(configurator);
        }

        public static CascadingConfigurator Unload()
        {
            return current.Unload();
        }
    }
}
