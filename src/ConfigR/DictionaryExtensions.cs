// <copyright file="DictionaryExtensions.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;

    public static class DictionaryExtensions
    {
        // NOTE: anonymous
        public static void Add(this IDictionary<string, object> dictionary, object value)
        {
            Guard.AgainstNullArgument("dictionary", dictionary);

            dictionary.Add(Guid.NewGuid().ToString(), value);
        }

        public static object Get(this IDictionary<string, object> dictionary)
        {
            Guard.AgainstNullArgument("dictionary", dictionary);

            if (dictionary.Count == 0)
            {
                throw new ConfigurationErrorsException("No item found.");
            }

            return dictionary.First().Value;
        }

        // NOTE: anonymous with casting
        public static T Get<T>(this IDictionary<string, object> dictionary)
        {
            Guard.AgainstNullArgument("dictionary", dictionary);

            T value;
            if (!dictionary.TryGetValue<T>(out value))
            {
                throw new ConfigurationErrorsException("Type not found.");
            }

            return value;
        }

        public static T GetOrDefault<T>(this IDictionary<string, object> dictionary)
        {
            Guard.AgainstNullArgument("dictionary", dictionary);

            T value;
            return dictionary.TryGetValue<T>(out value) ? value : default(T);
        }

        public static bool TryGetValue<T>(this IDictionary<string, object> dictionary, out T value)
        {
            Guard.AgainstNullArgument("dictionary", dictionary);

            value = default(T);
            foreach (var candidate in dictionary.Select(pair => pair.Value).Where(candidate => candidate != null))
            {
                if (typeof(T).IsAssignableFrom(candidate.GetType()))
                {
                    value = (T)candidate;
                    return true;
                }
            }

            return false;
        }

        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "Following design of IDictionary<T, V>.")]
        public static bool TryGetValueOrDefault<T>(this IDictionary<string, object> dictionary, out T value, T defaultValue)
        {
            Guard.AgainstNullArgument("dictionary", dictionary);

            if (dictionary.TryGetValue(out value))
            {
                return true;
            }

            value = defaultValue;
            return false;
        }

        // NOTE: keyed
        public static object GetOrDefault(this IDictionary<string, object> dictionary, string key)
        {
            Guard.AgainstNullArgument("dictionary", dictionary);

            return dictionary.GetOrDefault<object>(key);
        }

        // NOTE: keyed with casting
        public static T Get<T>(this IDictionary<string, object> dictionary, string key)
        {
            Guard.AgainstNullArgument("dictionary", dictionary);

            return dictionary[key].CastForRetreival<T>(key);
        }

        public static T GetOrDefault<T>(this IDictionary<string, object> dictionary, string key)
        {
            Guard.AgainstNullArgument("dictionary", dictionary);

            object value;
            return dictionary.TryGetValue(key, out value) ? value.CastForRetreival<T>(key) : default(T);
        }

        public static T GetOrDefault<T>(this IDictionary<string, object> dictionary, string key, T defaultValue)
        {
            Guard.AgainstNullArgument("dictionary", dictionary);

            object value;
            return dictionary.TryGetValue(key, out value) ? value.CastForRetreival<T>(key) : defaultValue;
        }

        public static bool TryGetValue<T>(this IDictionary<string, object> dictionary, string key, out T value)
        {
            Guard.AgainstNullArgument("dictionary", dictionary);

            value = default(T);
            object @object;
            if (!dictionary.TryGetValue(key, out @object))
            {
                return false;
            }

            value = @object.CastForRetreival<T>(key);
            return true;
        }

        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#", Justification = "'Advanced' feature.")]
        public static bool TryGetValueOrDefault<T>(this IDictionary<string, object> dictionary, string key, out T value, T defaultValue)
        {
            Guard.AgainstNullArgument("dictionary", dictionary);

            if (dictionary.TryGetValue(key, out value))
            {
                return true;
            }

            value = defaultValue;
            return false;
        }

        // NOTE: misc
        internal static TValue FriendlyGet<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            Guard.AgainstNullArgument("dictionary", dictionary);

            try
            {
                return dictionary[key];
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(string.Format(CultureInfo.InvariantCulture, "'{0}' not found.", key), ex);
            }
        }
    }
}
