// <copyright file="Loader.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Roslyn.CSharp.Internal
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using ConfigR.Sdk;
    using Microsoft.CodeAnalysis.CSharp.Scripting;
    using Microsoft.CodeAnalysis.Scripting;
    using Microsoft.CodeAnalysis.Scripting.Hosting;

    public class Loader : ILoader
    {
        private readonly string scriptPath;
        private readonly ScriptOptions options;
        private readonly InteractiveAssemblyLoader assemblyLoader;

        [CLSCompliant(false)]
        public Loader(string scriptPath = null, ScriptOptions options = null, InteractiveAssemblyLoader assemblyLoader = null)
        {
            this.scriptPath = scriptPath ??
                Path.ChangeExtension(AppDomain.CurrentDomain.SetupInformation.VSHostingAgnosticConfigurationFile(), "csx");

            this.options = options;
            this.assemblyLoader = assemblyLoader;
        }

        public async Task<DynamicDictionary> Load(DynamicDictionary config)
        {
            await CSharpScript.Create(
                    File.ReadAllText(this.scriptPath),
                    this.options ?? ScriptOptions.Default.ForConfigScript(this.scriptPath),
                    typeof(ScriptGlobals),
                    this.assemblyLoader)
                .RunAsync(new ScriptGlobals(config));

            return config;
        }
    }
}
