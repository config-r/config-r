// <copyright file="ConfigExtensions.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using ConfigR.Roslyn.CSharp.Internal;
    using ConfigR.Sdk;
    using Microsoft.CodeAnalysis.Scripting;
    using Microsoft.CodeAnalysis.Scripting.Hosting;

    public static class ConfigExtensions
    {
        [CLSCompliant(false)]
        public static IConfig UseRoslynCSharpLoader(
                this IConfig config,
                string scriptPath = null,
                ScriptOptions options = null,
                InteractiveAssemblyLoader assemblyLoader = null) =>
            config?.UseLoader(new Loader(scriptPath, options, assemblyLoader));
    }
}
