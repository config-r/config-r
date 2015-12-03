// <copyright file="ObjectExtensions.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.Globalization;

    internal static class ObjectExtensions
    {
        public static T CastForRetreival<T>(this object value, string key)
        {
            try
            {
                return (T)value;
            }
            catch (InvalidCastException ex)
            {
                var message = string.Format(
                    CultureInfo.InvariantCulture,
                    "'{0}' is of type '{1}' and cannot be cast to type '{2}'.",
                    key,
                    value == null ? "null" : value.GetType().ToString(),
                    typeof(T));

                throw new InvalidOperationException(message, ex);
            }
        }
    }
}
