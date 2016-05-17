// <copyright file="ScriptOptionsExtensions.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using ConfigR.Roslyn.CSharp.Internal;
    using ConfigR.Roslyn.CSharp.Logging;
    using Microsoft.CodeAnalysis.Scripting;

    public static class ScriptOptionsExtensions
    {
        private static readonly ILog log = LogProvider.GetCurrentClassLogger();

        [CLSCompliant(false)]
        public static ScriptOptions ForConfigScript(this ScriptOptions options) => options.ForConfigScript(null);

        [CLSCompliant(false)]
        public static ScriptOptions ForConfigScript(this ScriptOptions options, string scriptPath)
        {
            var searchPaths = new[]
            {
                Path.GetDirectoryName(scriptPath.ResolveScriptPath()).TrimEnd(Path.DirectorySeparatorChar),
                AppDomain.CurrentDomain.SetupInformation.ApplicationBase.TrimEnd(Path.DirectorySeparatorChar),
            }.Distinct().ToList();

            foreach (var searchPath in searchPaths.OrderBy(_ => _))
            {
                log.DebugFormat("Using search path '{0}'.", searchPath);
            }

            var references = new List<Assembly>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.IsDynamic)
                {
                    log.TraceFormat(
                        "Not adding a reference to assembly '{0}' in the script options because it is dynamic.",
                        assembly.FullName);

                    continue;
                }

                if (string.IsNullOrEmpty(assembly.Location))
                {
                    log.TraceFormat(
                        "Not adding a reference to assembly '{0}' in the script options because it has no location.",
                        assembly.FullName);

                    continue;
                }

                log.TraceFormat(
                    "Adding a reference to assembly '{0}' located at '{1}'.", assembly.FullName, assembly.Location);

                references.Add(assembly);
            }

            return options
                ?.WithMetadataResolver(ScriptMetadataResolver.Default.WithSearchPaths(searchPaths))
                .WithSourceResolver(ScriptSourceResolver.Default.WithSearchPaths(searchPaths))
                .AddReferences(references);
        }
    }
}
