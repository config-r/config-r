// <copyright file="ObjectExtensions.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using Common.Logging;

    internal static class ObjectExtensions
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Safe in this case.")]
        public static string TryToJsv(this object value)
        {
            try
            {
                return ServiceStack.Text.StringExtensions.ToJsv(value);
            }
            catch (Exception ex)
            {
                log.TraceFormat(CultureInfo.InvariantCulture, "Error converting '{0}' to JSV.", ex, value);
                return ServiceStack.Text.StringExtensions.ToJsv(value.GetType());
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
