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

    [CLSCompliant(false)]
    public class ConfigRScriptExecutor : ScriptExecutor
    {
        public ConfigRScriptExecutor(IFileSystem fileSystem, IFilePreProcessor filePreprocessor, IScriptEngine scriptEngine, ILog logger)
            : base(fileSystem, filePreprocessor, scriptEngine, logger)
        {
        }

        public override void Initialize(IEnumerable<string> paths, IEnumerable<IScriptPack> scriptPacks, params string[] scriptArgs)
        {
            base.Initialize(paths, scriptPacks, scriptArgs);

            // HACK (adamralph): BaseDirectory is set to bin subfolder in base.Initialize()!
            this.ScriptEngine.BaseDirectory = this.FileSystem.CurrentDirectory;
        }
    }
}
