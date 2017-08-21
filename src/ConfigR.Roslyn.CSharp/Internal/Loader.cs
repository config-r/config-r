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
    using System.Reflection;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis;

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
            SetCSharpVersionToLatest();
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

        private void SetCSharpVersionToLatest()
        {
            try
            {
                // reset default scripting mode to latest language version to enable C# 7.1 features
                // this is not needed once https://github.com/dotnet/roslyn/pull/21331 ships
                var csharpScriptCompilerType = typeof(CSharpScript).GetTypeInfo().Assembly.GetType("Microsoft.CodeAnalysis.CSharp.Scripting.CSharpScriptCompiler");
                var parseOptionsField = csharpScriptCompilerType?.GetField("s_defaultOptions", BindingFlags.Static | BindingFlags.NonPublic);
                parseOptionsField?.SetValue(null, new CSharpParseOptions(LanguageVersion.Latest, kind: SourceCodeKind.Script));
            }
            catch (Exception)
            {
                log.Warn("Unable to set C# language version to latest, will use the default version.");
            }
        }
    }
}
