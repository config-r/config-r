// <copyright file="RoslynCSharpLoader.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Roslyn.CSharp.Internal
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using ConfigR.Sdk;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Scripting;
    using Microsoft.CodeAnalysis.Scripting;
    using Microsoft.CodeAnalysis.Scripting.Hosting;

    public class RoslynCSharpLoader : ILoader
    {
        private readonly string scriptPath;
        private List<Assembly> references = new List<Assembly>();
        private List<MetadataReference> metadataReferences = new List<MetadataReference>();

        public RoslynCSharpLoader()
            : this(Path.ChangeExtension(
                AppDomain.CurrentDomain.SetupInformation.VSHostingAgnosticConfigurationFile(), "csx"))
        {
        }

        public RoslynCSharpLoader(string scriptPath)
        {
            this.scriptPath = scriptPath;
        }

        public async Task<DynamicDictionary> Load(DynamicDictionary config)
        {
            var code = File.ReadAllText(this.scriptPath);

            var searchPaths = new[]
            {
                Path.GetDirectoryName(Path.GetFullPath(this.scriptPath)),
                AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
            };

            var currentAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => !assembly.IsDynamic && !string.IsNullOrEmpty(assembly.Location));

            var options = ScriptOptions.Default
                .WithMetadataResolver(ScriptMetadataResolver.Default.WithSearchPaths(searchPaths))
                .WithSourceResolver(ScriptSourceResolver.Default.WithSearchPaths(searchPaths))
                .AddReferences(currentAssemblies.Concat(this.references))
                .AddReferences(this.metadataReferences);

            var assemblyLoader = new InteractiveAssemblyLoader();
            foreach (var reference in this.references
                .Where(reference => !reference.IsDynamic && string.IsNullOrEmpty(reference.Location)))
            {
                assemblyLoader.RegisterDependency(reference);
            }

            await CSharpScript.Create(code, options, typeof(ScriptGlobals), assemblyLoader).RunAsync(new ScriptGlobals(config));
            return config;
        }

        internal void AddReferences(Assembly[] references)
        {
            this.references.AddRange(references);
        }

        internal void AddReferences(MetadataReference[] references)
        {
            this.metadataReferences.AddRange(references);
        }
    }
}
