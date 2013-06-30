// <copyright file="ConfiguratorExtensions.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.Globalization;
    using Microsoft.CSharp.RuntimeBinder;

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

            return Get<T>(key, configurator[key]);
        }

        public static T GetOrDefault<T>(this IConfigurator configurator, string key)
        {
            Guard.AgainstNullArgument("configurator", configurator);

            dynamic value;
            return configurator.TryGet(key, out value) ? Get<T>(key, value) : default(T);
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

            value = Get<T>(key, dynamicValue);
            return true;
        }

        private static T Get<T>(string key, dynamic value)
        {
            try
            {
                return (T)value;
            }
            catch (RuntimeBinderException ex)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Error getting '{0}'.", key), ex);
            }
        }
    }
}
