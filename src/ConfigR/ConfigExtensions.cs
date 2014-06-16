// <copyright file="ConfigExtensions.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Common.Logging;

    public static class ConfigExtensions
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();

        public static IConfig LoadWebScript(this IConfig config, Uri uri, params Assembly[] references)
        {
            Guard.AgainstNullArgument("config", config);

            return config.Load(new WebScriptConfig(uri, references));
        }

        public static IConfig LoadScriptFile(this IConfig config, string path, params Assembly[] references)
        {
            Guard.AgainstNullArgument("config", config);

            return config.Load(new ScriptFileConfig(path, references));
        }

        public static IConfig LoadLocalScriptFile(this IConfig config, params Assembly[] references)
        {
            Guard.AgainstNullArgument("config", config);

            return config.Load(new LocalScriptFileConfig(references));
        }

        [Obsolete("Deprecated since version 0.9 and will soon be removed. Use LoadWebScript(Uri) instead.")]
        public static IConfig Load(this IConfig config, Uri uri)
        {
            log.Warn("ConfigR.IConfig.Load(Uri) is deprecated since version 0.9 and will soon be removed. Use LoadWebScript(Uri) instead.");
            return config.LoadWebScript(uri);
        }

        [Obsolete("Deprecated since version 0.9 and will soon be removed. LoadScriptFile(string) instead.")]
        [SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads", Justification = "It's not a string URI, it's a path.")]
        public static IConfig Load(this IConfig config, string path)
        {
            log.Warn("ConfigR.IConfig.Load(string) is deprecated since version 0.9 and will soon be removed. Use LoadScriptFile(string) instead.");
            return config.LoadScriptFile(path);
        }

        [Obsolete("Deprecated since version 0.9 and will soon be removed. Use LoadLocalScriptFile() instead.")]
        public static IConfig LoadLocal(this IConfig config)
        {
            log.Warn("ConfigR.IConfig.LoadLocal() is deprecated since version 0.9 and will soon be removed. Use LoadLocalScriptFile() instead.");
            return config.LoadLocalScriptFile();
        }
    }
}
