// <copyright file="Loader.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Roslyn.CSharp.Internal
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using ConfigR.Sdk;
    using Microsoft.CodeAnalysis.CSharp.Scripting;
    using Microsoft.CodeAnalysis.Scripting;

    public class Loader : ILoader
    {
        public async Task<DynamicDictionary> Load(DynamicDictionary config)
        {
            var path = Path.ChangeExtension(
                AppDomain.CurrentDomain.SetupInformation.VSHostingAgnosticConfigurationFile(), "csx");

            var code = File.ReadAllText(path);

            var searchPaths = new[]
            {
                Path.GetDirectoryName(Path.GetFullPath(path)),
                AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
            };

            var references = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => !assembly.IsDynamic && !string.IsNullOrEmpty(assembly.Location));

            var options = ScriptOptions.Default
                .WithMetadataResolver(ScriptMetadataResolver.Default.WithSearchPaths(searchPaths))
                .WithSourceResolver(ScriptSourceResolver.Default.WithSearchPaths(searchPaths))
                .AddReferences(references);

            await CSharpScript.Create(code, options, typeof(ScriptGlobals)).RunAsync(new ScriptGlobals(config));
            return config;
        }
    }
}
