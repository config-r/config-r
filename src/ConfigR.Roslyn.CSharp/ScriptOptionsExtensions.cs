// <copyright file="ScriptOptionsExtensions.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using ConfigR.Roslyn.CSharp.Internal;
    using Microsoft.CodeAnalysis.Scripting;

    public static class ScriptOptionsExtensions
    {
        [CLSCompliant(false)]
        public static ScriptOptions ForConfigScript(this ScriptOptions options) =>
            options.ForConfigScript(null);

        [CLSCompliant(false)]
        public static ScriptOptions ForConfigScript(this ScriptOptions options, string address) =>
            options.ForConfigScript(address.ResolveScriptUri());
    }
}
