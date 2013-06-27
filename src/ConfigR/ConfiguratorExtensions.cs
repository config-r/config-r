// <copyright file="ConfiguratorExtensions.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System.Diagnostics.CodeAnalysis;

    public static class ConfiguratorExtensions
    {
        public static dynamic Get(this IConfigurator configurator, string key)
        {
            return configurator.Get<dynamic>(key);
        }

        public static dynamic GetOrDefault(this IConfigurator configurator, string key)
        {
            return configurator.GetOrDefault<dynamic>(key);
        }

        public static T Get<T>(this IConfigurator configurator, string key)
        {
            Guard.AgainstNullArgument("configurator", configurator);

            return (T)configurator[key];
        }

        public static T GetOrDefault<T>(this IConfigurator configurator, string key)
        {
            Guard.AgainstNullArgument("configurator", configurator);

            dynamic value;
            return configurator.TryGet(key, out value) ? (T)value : default(T);
        }

        public static bool TryGet<T>(this IConfigurator configurator, string key, out T value)
        {
            Guard.AgainstNullArgument("configurator", configurator);

            value = default(T);
            dynamic dynamicValue;
            if (!configurator.TryGet(key, out dynamicValue))
            {
                return false;
            }

            value = (T)dynamicValue;
            return true;
        }
    }
}
