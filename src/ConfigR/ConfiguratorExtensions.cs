// <copyright file="ConfiguratorExtensions.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.Linq;
    using Microsoft.CSharp.RuntimeBinder;

    public static class ConfiguratorExtensions
    {
        public static IConfigurator Add(this IConfigurator configurator, dynamic value)
        {
            return configurator.Add(Guid.NewGuid().ToString(), value);
        }

        public static T Get<T>(this IConfigurator configurator)
        {
            T value;
            if (!configurator.TryGet<T>(out value))
            {
                throw new ConfigurationErrorsException("Type not found.");
            }

            return value;
        }

        public static T GetOrDefault<T>(this IConfigurator configurator)
        {
            T value;
            return configurator.TryGet<T>(out value) ? value : default(T);
        }

        public static bool TryGet<T>(this IConfigurator configurator, out T value)
        {
            Guard.AgainstNullArgument("configurator", configurator);

            value = default(T);
            foreach (var candidate in configurator.Items.Select(pair => pair.Value).Where(candidate => candidate != null))
            {
                if (typeof(T).IsAssignableFrom(candidate.GetType()))
                {
                    value = candidate;
                    return true;
                }
            }

            return false;
        }

        public static dynamic Get(this IConfigurator configurator, string key)
        {
            return configurator.Get<dynamic>(key);
        }

        public static dynamic GetOrDefault(this IConfigurator configurator, string key)
        {
            return configurator.GetOrDefault<dynamic>(key);
        }

        public static bool TryGet(this IConfigurator configurator, string key, out dynamic value)
        {
            return configurator.TryGet<dynamic>(key, out value);
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
