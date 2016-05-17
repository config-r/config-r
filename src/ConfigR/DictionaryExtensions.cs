// <copyright file="DictionaryExtensions.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System.Collections.Generic;
    using ConfigR.Internal;

    public static class DictionaryExtensions
    {
        public static T Get<T>(this IDictionary<string, object> dictionary, string key)
        {
            Guard.AgainstNullArgument("dictionary", dictionary);

            return dictionary[key].CastForRetrieval<T>(key);
        }

        public static T Get<T>(this IDictionary<string, object> dictionary, string key, T defaultValue)
        {
            Guard.AgainstNullArgument("dictionary", dictionary);

            object value;
            return dictionary.TryGetValue(key, out value) ? value.CastForRetrieval<T>(key) : defaultValue;
        }
    }
}
