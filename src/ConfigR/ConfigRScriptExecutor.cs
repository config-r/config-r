// <copyright file="ConfigRScriptExecutor.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
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
        private static readonly ILog log = LogManager.GetCurrentClassLogger();
        private static readonly ILog scriptCsLog = LogManager.GetLogger("ScriptCs");

        public ConfigRScriptExecutor(IFileSystem fileSystem)
            : base(
            fileSystem,
            new FilePreProcessor(fileSystem, scriptCsLog, new ILineProcessor[] { new LoadLineProcessor(fileSystem) }),
            new RoslynScriptEngine(new ScriptHostFactory(), scriptCsLog),
            scriptCsLog)
        {
        }

        public override void Initialize(IEnumerable<string> paths, IEnumerable<IScriptPack> scriptPacks, params string[] scriptArgs)
        {
            base.Initialize(paths, scriptPacks, scriptArgs);
            this.ScriptEngine.BaseDirectory = this.FileSystem.CurrentDirectory; // NOTE (adamralph): set to bin subfolder in base.Initialize()!
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
            this.Terminate();
        }

        private static void RethrowExceptionIfAny(ScriptResult result, string script)
        {
            if (result.CompileExceptionInfo != null)
            {
                log.ErrorFormat("Failed to compile {0}", result.CompileExceptionInfo, script);
                result.CompileExceptionInfo.Throw();
            }

            if (result.ExecuteExceptionInfo != null)
            {
                log.ErrorFormat("Failed to execute {0}", result.ExecuteExceptionInfo, script);
                result.ExecuteExceptionInfo.Throw();
            }
        }
    }
}
