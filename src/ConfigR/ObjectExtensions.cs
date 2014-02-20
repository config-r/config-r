// <copyright file="ObjectExtensions.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using Common.Logging;
    using Newtonsoft.Json;

    internal static class ObjectExtensions
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();
        private static readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            MaxDepth = 4
        };

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Safe in this case.")]
        public static string ToJson(this object value)
        {
            try
            {
                return JsonConvert.SerializeObject(value, Formatting.None, jsonSettings);
            }
            catch (Exception ex)
            {
                log.TraceFormat(CultureInfo.InvariantCulture, "Error converting '{0}' to JSON.", ex, value);
                return JsonConvert.SerializeObject(value.GetType(), Formatting.None, jsonSettings);
            }
        }

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
