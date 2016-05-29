// <copyright file="Loader.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Roslyn.CSharp.Internal
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using ConfigR.Roslyn.CSharp.Logging;
    using ConfigR.Sdk;
    using Microsoft.CodeAnalysis.CSharp.Scripting;
    using Microsoft.CodeAnalysis.Scripting;
    using Microsoft.CodeAnalysis.Scripting.Hosting;

    public class Loader : ILoader
    {
        private static readonly ILog log = LogProvider.GetCurrentClassLogger();

        private readonly Uri scriptUri;
        private readonly ScriptOptions options;
        private readonly InteractiveAssemblyLoader assemblyLoader;

        [CLSCompliant(false)]
        public Loader(Uri scriptUri, ScriptOptions options, InteractiveAssemblyLoader assemblyLoader)
        {
            this.scriptUri = scriptUri;
            this.options = options;
            this.assemblyLoader = assemblyLoader;
        }

        public async Task<DynamicDictionary> Load(DynamicDictionary config)
        {
            var path = this.scriptUri.ToFilePath();

            log.InfoFormat("Running script '{0}'...", path);
            await CSharpScript.Create(
                    File.ReadAllText(path), this.options, typeof(ScriptGlobals), this.assemblyLoader)
                .RunAsync(new ScriptGlobals(config));

            return config;
        }
    }
}
