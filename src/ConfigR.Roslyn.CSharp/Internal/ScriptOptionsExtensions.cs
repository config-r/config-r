// <copyright file="ScriptOptionsExtensions.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Roslyn.CSharp.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.IO;
    using System.Reflection;
    using ConfigR.Roslyn.CSharp.Logging;
    using Microsoft.CodeAnalysis.Scripting;

    public static class ScriptOptionsExtensions
    {
        private static readonly ILog log = LogProvider.GetCurrentClassLogger();

        [CLSCompliant(false)]
        public static ScriptOptions ForConfigScript(this ScriptOptions options, Uri uri)
        {
            var searchPaths = new SortedSet<string>();

            string path;
            if (uri.TryGetFilePath(out path))
            {
                searchPaths.Add(Path.GetDirectoryName(path).TrimEnd(Path.DirectorySeparatorChar));
            }

            searchPaths.Add(AppDomain.CurrentDomain.SetupInformation.ApplicationBase.TrimEnd(Path.DirectorySeparatorChar));

            foreach (var searchPath in searchPaths)
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

            var sourceResolver =
                new HttpSourceFileResolver(searchPaths.ToImmutableArray(), ScriptSourceResolver.Default.BaseDirectory);

            return options
                ?.WithMetadataResolver(ScriptMetadataResolver.Default.WithSearchPaths(searchPaths))
                .WithSourceResolver(sourceResolver)
                .AddReferences(references);
        }
    }
}
