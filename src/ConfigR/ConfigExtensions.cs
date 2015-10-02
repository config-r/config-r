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
    }
}
