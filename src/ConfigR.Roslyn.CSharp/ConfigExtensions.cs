// <copyright file="ConfigExtensions.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using ConfigR.Roslyn.CSharp;
    using ConfigR.Roslyn.CSharp.Internal;
    using ConfigR.Sdk;

    public static class ConfigExtensions
    {
        [CLSCompliant(false)]
        public static IRoslynCSharpConfig UseRoslynCSharpLoader(this IConfig config)
        {
            var loader = new RoslynCSharpLoader();
            return new RoslynCSharpConfig(config?.UseLoader(loader), loader);
        }

        [CLSCompliant(false)]
        public static IRoslynCSharpConfig UseRoslynCSharpLoader(this IConfig config, string scriptPath)
        {
            var loader = new RoslynCSharpLoader(scriptPath);
            return new RoslynCSharpConfig(config?.UseLoader(loader), loader);
        }
    }
}
