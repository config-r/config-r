// <copyright file="Configurator.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Common.Logging;

    [Obsolete("Deprecated since version 0.9 and will soon be removed. Use Config.Global instead.")]
    public static class Configurator
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();

        public static IConfig Current
        {
            get
            {
                LogObsolete();
                return Config.Global;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Necessary.")]
        public static IEnumerable<KeyValuePair<string, dynamic>> Items
        {
            get
            {
                LogObsolete();
                return Config.Global;
            }
        }

        public static IConfig Add(string key, object value)
        {
            LogObsolete();
            Config.DisableGlobalAutoLoading().Add(key, value);
            return Config.Global;
        }

        public static IConfig Add(object value)
        {
            LogObsolete();
            Config.DisableGlobalAutoLoading().Add(value as object);
            return Config.Global;
        }

        public static T Get<T>()
        {
            LogObsolete();
            return Config.Global.Get<T>();
        }

        public static T GetOrDefault<T>()
        {
            LogObsolete();
            return Config.Global.GetOrDefault<T>();
        }

        public static bool TryGet<T>(out T value)
        {
            LogObsolete();
            return Config.Global.TryGetValue<T>(out value);
        }

        public static dynamic Get(string key)
        {
            LogObsolete();
            return Config.Global[key];
        }

        public static dynamic GetOrDefault(string key)
        {
            LogObsolete();
            return Config.Global.GetOrDefault(key);
        }

        public static T Get<T>(string key)
        {
            LogObsolete();
            return Config.Global.Get<T>(key);
        }

        public static T GetOrDefault<T>(string key)
        {
            LogObsolete();
            return Config.Global.GetOrDefault<T>(key);
        }

        public static bool TryGet<T>(string key, out T value)
        {
            LogObsolete();
            return Config.Global.TryGetValue<T>(key, out value);
        }

        public static T GetOrDefault<T>(string key, T defaultValue)
        {
            LogObsolete();
            return Config.Global.GetOrDefault(key, defaultValue);
        }

        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "'Advanced' feature.")]
        public static bool TryGetOrDefault<T>(string key, out T value, T defaultValue)
        {
            LogObsolete();
            return Config.Global.TryGetValueOrDefault(key, out value, defaultValue);
        }

        public static IConfig Load(Uri uri)
        {
            LogObsolete();
            return Config.DisableGlobalAutoLoading().LoadWebScript(uri);
        }

        [SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads", Justification = "It's not a string URI, it's a path.")]
        public static IConfig Load(string path)
        {
            LogObsolete();
            return Config.DisableGlobalAutoLoading().LoadScriptFile(path);
        }

        public static IConfig LoadLocal()
        {
            LogObsolete();
            return Config.DisableGlobalAutoLoading().LoadLocalScriptFile();
        }

        public static IConfig Load(ISimpleConfig config)
        {
            LogObsolete();
            return Config.DisableGlobalAutoLoading().Load(config);
        }

        private static void LogObsolete()
        {
            log.Warn("ConfigR.Configurator is deprecated since version 0.9 and will soon be removed. Use Config and Config.Global instead.");
        }
    }
}
