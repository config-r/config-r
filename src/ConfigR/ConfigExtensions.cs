// <copyright file="ConfigExtensions.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.Reflection;

    public static class ConfigExtensions
    {
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
