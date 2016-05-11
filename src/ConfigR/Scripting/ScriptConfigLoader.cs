// <copyright file="ScriptConfigLoader.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Scripting.Hosting;

namespace ConfigR.Scripting
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using ConfigR.Logging;
    using Microsoft.CodeAnalysis.CSharp.Scripting;
    using Microsoft.CodeAnalysis.Scripting;

    public class ScriptConfigLoader
    {
        private static readonly Logging.ILog log = LogProvider.For<ScriptConfigLoader>();
        private readonly Assembly[] references;
        private readonly Assembly[] inMemoryReferences;
        private readonly MetadataReference[] metadataReferences;

        public ScriptConfigLoader(params Assembly[] references) : this(references, new MetadataReference[0])
        {
        }

#pragma warning disable CS3001
        public ScriptConfigLoader(Assembly[] references, MetadataReference[] metadataReferences)
#pragma warning restore CS3001
        {
            this.references = (references?.Where(x => !string.IsNullOrWhiteSpace(x.Location)) ?? Enumerable.Empty<Assembly>()).ToArray();
            this.inMemoryReferences = (references?.Where(x => string.IsNullOrWhiteSpace(x.Location)) ?? Enumerable.Empty<Assembly>()).ToArray();
            this.metadataReferences = metadataReferences ?? new MetadataReference[0];
        }

        public object LoadFromFile(ISimpleConfig config, string path)
        {
            Guard.AgainstNullArgument(nameof(config), config);

            path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, path);
            var code = File.ReadAllText(Path.Combine(path));

            var searchPaths = new[]
            {
                Path.GetDirectoryName(Path.GetFullPath(path)),
                AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
            };

            var options = ScriptOptions.Default
                .AddReferences(this.metadataReferences)
                .WithMetadataResolver(ScriptMetadataResolver.Default.WithSearchPaths(searchPaths))
                .WithSourceResolver(ScriptSourceResolver.Default.WithSearchPaths(searchPaths))
                .AddReferences(typeof(Config).Assembly)
                .AddReferences(this.references)
                .AddImports("System", "System.Collections.Generic", "System.IO", "System.Linq", typeof(Config).Namespace);

            if (inMemoryReferences.Any())
            {
                using (var interactiveLoader = new InteractiveAssemblyLoader())
                {
                    foreach (var inMemoryReference in inMemoryReferences)
                    {
                        interactiveLoader.RegisterDependency(inMemoryReference);
                    }
                    return CSharpScript
                        .Create(code, options, typeof (ConfigRScriptHost), interactiveLoader)
                        .RunAsync(new ConfigRScriptHost(config)).GetAwaiter().GetResult()
                        .ReturnValue;
                }
            }

            return CSharpScript
                .Create(code, options, typeof (ConfigRScriptHost))
                .RunAsync(new ConfigRScriptHost(config)).GetAwaiter().GetResult()
                .ReturnValue;
        }
    }
}
