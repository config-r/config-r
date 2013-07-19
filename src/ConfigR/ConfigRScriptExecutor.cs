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
            : base(fileSystem, new FilePreProcessor(fileSystem, scriptCsLog), new RoslynScriptEngine(new ScriptHostFactory(), scriptCsLog), scriptCsLog)
        {
        }

        public override void Initialize(IEnumerable<string> paths, IEnumerable<IScriptPack> scriptPacks)
        {
            base.Initialize(paths, scriptPacks);
            this.ScriptEngine.BaseDirectory = this.FileSystem.CurrentDirectory; // NOTE (adamralph): set to bin subfolder in base.Initialize()!
        }

        public override ScriptResult Execute(string script)
        {
            return this.Execute(script, new string[0]);
        }

        public override ScriptResult Execute(string script, string[] scriptArgs)
        {
            var result = base.Execute(script, scriptArgs);
            if (result.CompileException != null)
            {
                log.ErrorFormat("Failed to compile {0}", result.CompileException, script);
                result.CompileException.RethrowWithNoStackTraceLoss();
            }

            if (result.ExecuteException != null)
            {
                log.ErrorFormat("Failed to execute {0}", result.ExecuteException, script);
                result.ExecuteException.RethrowWithNoStackTraceLoss();
            }

            return result;
        }

        public void Dispose()
        {
            this.Terminate();
        }
    }
}
