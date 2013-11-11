// <copyright file="ConfigurationExtensions.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using Microsoft.CSharp.RuntimeBinder;

    public static class ConfigurationExtensions
    {
        public static T Get<T>(this IConfiguration configuration)
        {
            T value;
            if (!configuration.TryGet<T>(out value))
            {
                throw new ConfigurationErrorsException("Type not found.");
            }

            return value;
        }

        public static T GetOrDefault<T>(this IConfiguration configuration)
        {
            T value;
            return configuration.TryGet<T>(out value) ? value : default(T);
        }

        public static bool TryGet<T>(this IConfiguration configuration, out T value)
        {
            Guard.AgainstNullArgument("configuration", configuration);

            value = default(T);
            foreach (var candidate in configuration.Items.Select(pair => pair.Value).Where(candidate => candidate != null))
            {
                if (typeof(T).IsAssignableFrom(candidate.GetType()))
                {
                    value = candidate;
                    return true;
                }
            }

            return false;
        }

        public static dynamic Get(this IConfiguration configuration, string key)
        {
            return configuration.Get<dynamic>(key);
        }

        public static dynamic GetOrDefault(this IConfiguration configuration, string key)
        {
            return configuration.GetOrDefault<dynamic>(key);
        }

        public static T Get<T>(this IConfiguration configuration, string key)
        {
            Guard.AgainstNullArgument("configuration", configuration);

            return Cast<T>(configuration[key], key);
        }

        public static T GetOrDefault<T>(this IConfiguration configuration, string key)
        {
            Guard.AgainstNullArgument("configuration", configuration);

            dynamic value;
            return configuration.TryGet(key, out value) ? Cast<T>(value, key) : default(T);
        }

        public static bool TryGet<T>(this IConfiguration configuration, string key, out T value)
        {
            Guard.AgainstNullArgument("configuration", configuration);

            value = default(T);
            dynamic dynamicValue;
            if (!configuration.TryGet(key, out dynamicValue))
            {
                return false;
            }

            value = Cast<T>(dynamicValue, key);
            return true;
        }

        public static T GetOrDefault<T>(this IConfiguration configuration, string key, T defaultValue)
        {
            Guard.AgainstNullArgument("configuration", configuration);

            dynamic value;
            return configuration.TryGet(key, out value) ? Cast<T>(value, key) : defaultValue;
        }

        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#", Justification = "'Advanced' feature.")]
        public static bool TryGetOrDefault<T>(this IConfiguration configuration, string key, out T value, T defaultValue)
        {
            Guard.AgainstNullArgument("configuration", configuration);

            if (configuration.TryGet(key, out value))
            {
                return true;
            }

            value = defaultValue;
            return false;
        }

        private static T Cast<T>(dynamic value, string key)
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
