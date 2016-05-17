// <copyright file="ConfigExtensions.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using ConfigR.Roslyn.CSharp.Internal;
    using ConfigR.Sdk;
    using Microsoft.CodeAnalysis.Scripting;
    using Microsoft.CodeAnalysis.Scripting.Hosting;

    public static class ConfigExtensions
    {
        [SuppressMessage(
            "Microsoft.Design",
            "CA1026:DefaultParametersShouldNotBeUsed",
            Justification = "Too many combinations. Following the Roslyn API.")]
        [CLSCompliant(false)]
        public static IConfig UseRoslynCSharpLoader(
                this IConfig config,
                string scriptPath = null,
                ScriptOptions options = null,
                InteractiveAssemblyLoader assemblyLoader = null) =>
            config?.UseLoader(
                new Loader(
                    scriptPath.ResolveScriptPath(),
                    options ?? ScriptOptions.Default.ForConfigScript(scriptPath),
                    assemblyLoader));
    }
}
