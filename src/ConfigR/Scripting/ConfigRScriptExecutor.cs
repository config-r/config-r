// <copyright file="ConfigRScriptExecutor.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Scripting
{
    using System;
    using System.Collections.Generic;
    using Common.Logging;
    using ScriptCs;
    using ScriptCs.Contracts;
    using ScriptCs.Engine.Roslyn;

    [CLSCompliant(false)]
    public sealed class ConfigRScriptExecutor : ScriptExecutor, IDisposable
    {
        private static readonly ILog scriptCsLog = LogManager.GetLogger("ScriptCs");
        private bool isInitialized;

        public ConfigRScriptExecutor(ISimpleConfig config, IFileSystem fileSystem)
            : base(
                fileSystem,
                new FilePreProcessor(fileSystem, scriptCsLog, new ILineProcessor[] { new LoadLineProcessor(fileSystem), new ReferenceLineProcessor(fileSystem), new UsingLineProcessor() }),
                new RoslynScriptInMemoryEngine(new ConfigRScriptHostFactory(config), scriptCsLog),
                scriptCsLog)
        {
        }

        public override void Initialize(IEnumerable<string> paths, IEnumerable<IScriptPack> scriptPacks, params string[] scriptArgs)
        {
            base.Initialize(paths, scriptPacks, scriptArgs);
            this.ScriptEngine.BaseDirectory = this.FileSystem.CurrentDirectory; // NOTE (adamralph): set to bin subfolder in base.Initialize()!
            this.isInitialized = true;
        }

        public void Dispose()
        {
            if (this.isInitialized)
            {
                this.Terminate();
                this.isInitialized = false;
            }
        }
    }
}
