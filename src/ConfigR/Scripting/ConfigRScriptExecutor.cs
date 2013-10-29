// <copyright file="ConfigRScriptExecutor.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Scripting
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Common.Logging;
    using ScriptCs;
    using ScriptCs.Contracts;

    [CLSCompliant(false)]
    public sealed class ConfigRScriptExecutor : ScriptExecutor, IDisposable
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();
        private static readonly ILog scriptCsLog = LogManager.GetLogger("ScriptCs");
        private bool isInitialized;

        public ConfigRScriptExecutor(IConfigurator configurator, IFileSystem fileSystem)
            : base(
                fileSystem,
                new FilePreProcessor(fileSystem, scriptCsLog, new ILineProcessor[] { new LoadLineProcessor(fileSystem) }),
                new ConfigRScriptEngine(configurator, new ConfigRScriptHostFactory(), scriptCsLog),
                scriptCsLog)
        {
        }

        public override void Initialize(IEnumerable<string> paths, IEnumerable<IScriptPack> scriptPacks, params string[] scriptArgs)
        {
            base.Initialize(paths, scriptPacks, scriptArgs);
            this.ScriptEngine.BaseDirectory = this.FileSystem.CurrentDirectory; // NOTE (adamralph): set to bin subfolder in base.Initialize()!
            this.isInitialized = true;
        }

        public override ScriptResult ExecuteScript(string script, params string[] scriptArgs)
        {
            var result = base.ExecuteScript(script, scriptArgs);
            RethrowExceptionIfAny(result, script);
            return result;
        }

        public override ScriptResult Execute(string script, string[] scriptArgs)
        {
            var result = base.Execute(script, scriptArgs);
            RethrowExceptionIfAny(result, script);
            return result;
        }

        public void Dispose()
        {
            if (this.isInitialized)
            {
                this.Terminate();
                this.isInitialized = false;
            }
        }

        private static void RethrowExceptionIfAny(ScriptResult result, string script)
        {
            if (result.CompileExceptionInfo != null)
            {
                log.ErrorFormat(CultureInfo.InvariantCulture, "Failed to compile {0}", result.CompileExceptionInfo, script);
                result.CompileExceptionInfo.Throw();
            }

            if (result.ExecuteExceptionInfo != null)
            {
                log.ErrorFormat(CultureInfo.InvariantCulture, "Failed to execute {0}", result.ExecuteExceptionInfo, script);
                result.ExecuteExceptionInfo.Throw();
            }
        }
    }
}
