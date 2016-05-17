// <copyright file="ScriptOptionsExtensions.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.IO;
    using System.Linq;
    using Microsoft.CodeAnalysis.Scripting;

    public static class ScriptOptionsExtensions
    {
        [CLSCompliant(false)]
        public static ScriptOptions ForConfigScript(this ScriptOptions options, string scriptPath = null)
        {
            var searchPaths = new[]
            {
                Path.GetDirectoryName(Path.GetFullPath(scriptPath ?? AppDomain.CurrentDomain.SetupInformation.ConfigurationFile)),
                AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
            };

            var currentAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => !assembly.IsDynamic && !string.IsNullOrEmpty(assembly.Location));

            return options
                ?.WithMetadataResolver(ScriptMetadataResolver.Default.WithSearchPaths(searchPaths))
                .WithSourceResolver(ScriptSourceResolver.Default.WithSearchPaths(searchPaths))
                .AddReferences(currentAssemblies);
        }
    }
}
