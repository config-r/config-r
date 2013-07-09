// <copyright file="ExceptionExtensions.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.Reflection;

    public static class ExceptionExtensions
    {
        private static readonly string monoRethrowMarker = string.Concat(Environment.NewLine, "$$RethrowMarker$$", Environment.NewLine);

        /// <summary>
        /// Re-throws an exception object without losing the existing stack trace information.
        /// </summary>
        /// <param name="exception">The exception to re-throw.</param>
        /// <remarks>
        /// The remote_stack_trace string is here to support Mono.
        /// Borrowed from xUnit.net.
        /// </remarks>
        public static void RethrowWithNoStackTraceLoss(this Exception exception)
        {
            // TODO (xUnit.net): Is there code from ASP.NET Web Stack that we can borrow, that helps us do better things in 4.5?
            FieldInfo remoteStackTraceString =
                typeof(Exception).GetField("_remoteStackTraceString", BindingFlags.Instance | BindingFlags.NonPublic) ??
                typeof(Exception).GetField("remote_stack_trace", BindingFlags.Instance | BindingFlags.NonPublic);

            remoteStackTraceString.SetValue(exception, exception.StackTrace + monoRethrowMarker);
            throw exception;
        }
    }
}
