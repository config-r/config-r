// <copyright file="ObjectExtensions.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Internal
{
    using System;
    using static System.FormattableString;

    public static class ObjectExtensions
    {
        public static T CastForRetrieval<T>(this object value, string key)
        {
            try
            {
                return (T)value;
            }
            catch (NullReferenceException ex)
            {
                throw new InvalidOperationException(
                    Invariant($"'{key}' is null and cannot be cast to type '{typeof(T)}'."), ex);
            }
            catch (InvalidCastException ex)
            {
                throw new InvalidOperationException(
                    Invariant($"'{key}' is of type '{value.GetType().ToString()}' and cannot be cast to type '{typeof(T).ToString()}'."),
                    ex);
            }
        }
    }
}
