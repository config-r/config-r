// <copyright file="Configurator.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public static class Configurator
    {
        private static CascadingConfigurator current = new CascadingConfigurator();

        public static ICascadingConfigurator Current
        {
            get
            {
                EnsureLoaded();
                return current;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Necessary.")]
        public static IEnumerable<KeyValuePair<string, dynamic>> Items
        {
            get
            {
                EnsureLoaded();
                return current.Items;
            }
        }

        public static ICascadingConfigurator Add(string key, dynamic value)
        {
            return current.Add(key, value);
        }

        public static ICascadingConfigurator Add(dynamic value)
        {
            return current.Add(value as object);
        }

        public static T Get<T>()
        {
            EnsureLoaded();
            return current.Get<T>();
        }

        public static T GetOrDefault<T>()
        {
            EnsureLoaded();
            return current.GetOrDefault<T>();
        }

        public static bool TryGet<T>(out T value)
        {
            EnsureLoaded();
            return current.TryGet<T>(out value);
        }

        public static dynamic Get(string key)
        {
            EnsureLoaded();
            return current[key];
        }

        public static dynamic GetOrDefault(string key)
        {
            EnsureLoaded();
            return current.GetOrDefault(key);
        }

        public static T Get<T>(string key)
        {
            EnsureLoaded();
            return current.Get<T>(key);
        }

        public static T GetOrDefault<T>(string key)
        {
            EnsureLoaded();
            return current.GetOrDefault<T>(key);
        }

        public static bool TryGet<T>(string key, out T value)
        {
            EnsureLoaded();
            return current.TryGet<T>(key, out value);
        }

        public static T GetOrDefault<T>(string key, T defaultValue)
        {
            return current.GetOrDefault(key, defaultValue);
        }

        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "'Advanced' feature.")]
        public static bool TryGetOrDefault<T>(string key, out T value, T defaultValue)
        {
            return current.TryGetOrDefault(key, out value, defaultValue);
        }

        public static ICascadingConfigurator Load(Uri uri)
        {
            return current.Load(uri);
        }

        [SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads", Justification = "It's not a string URI, it's a path.")]
        public static ICascadingConfigurator Load(string path)
        {
            return current.Load(path);
        }

        public static ICascadingConfigurator LoadLocal()
        {
            return current.LoadLocal();
        }

        public static ICascadingConfigurator Load(IConfigurator configurator)
        {
            return current.Load(configurator);
        }

        public static ICascadingConfigurator Unload()
        {
            return current = new CascadingConfigurator();
        }

        private static void EnsureLoaded()
        {
            if (!current.LoadAttempted)
            {
                current.LoadLocal();
            }
        }
    }
}
