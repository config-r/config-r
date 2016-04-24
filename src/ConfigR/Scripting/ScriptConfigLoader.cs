// <copyright file="ScriptConfigLoader.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

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

        public ScriptConfigLoader(params Assembly[] references)
        {
            this.references = (references ?? Enumerable.Empty<Assembly>()).ToArray();
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
                .WithMetadataResolver(ScriptMetadataResolver.Default.WithSearchPaths(searchPaths))
                .WithSourceResolver(ScriptSourceResolver.Default.WithSearchPaths(searchPaths))
                .AddReferences(typeof(Config).Assembly)
                .AddReferences(this.references)
                .AddImports("System", "System.Collections.Generic", "System.IO", "System.Linq", typeof(Config).Namespace);

            return CSharpScript
                .Create(code, options, typeof(ConfigRScriptHost))
                .RunAsync(new ConfigRScriptHost(config)).GetAwaiter().GetResult()
                .ReturnValue;
        }
    }
}
