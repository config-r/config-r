// <copyright file="DictionaryExtensions.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.Collections.Generic;

    public static class DictionaryExtensions
    {
        public static T Get<T>(this IDictionary<string, object> dictionary, string key)
        {
            Guard.AgainstNullArgument("dictionary", dictionary);

            return dictionary[key].CastForRetreival<T>(key);
        }

        public static T Get<T>(this IDictionary<string, object> dictionary, string key, T defaultValue)
        {
            Guard.AgainstNullArgument("dictionary", dictionary);

            object value;
            return dictionary.TryGetValue(key, out value) ? value.CastForRetreival<T>(key) : defaultValue;
        }

        private static T CastForRetreival<T>(this object value, string key)
        {
            try
            {
                return (T)value;
            }
            catch (NullReferenceException ex)
            {
                throw new InvalidOperationException($"'{key}' is null and cannot be cast to type '{typeof(T)}'.", ex);
            }
            catch (InvalidCastException ex)
            {
                throw new InvalidOperationException(
                    $"'{key}' is of type '{value.GetType().ToString()}' and cannot be cast to type '{typeof(T).ToString()}'.", ex);
            }
        }
    }
}
