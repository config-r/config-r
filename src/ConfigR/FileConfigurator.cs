// <copyright file="FileConfigurator.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.Globalization;
    using Common.Logging;
    using ScriptCs;
    using ScriptCs.Engine.Roslyn;

    public class FileConfigurator : BasicConfigurator
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();
        private readonly string path;

        public FileConfigurator(string path)
        {
            this.path = path;
        }

        public string Path
        {
            get { return this.path; }
        }

        public override IConfigurator Load()
        {
            var fileSystem = new ConfigRFileSystem(new FileSystem());
            log.DebugFormat(CultureInfo.InvariantCulture, "Initialized file system with current directory {0}", fileSystem.CurrentDirectory);

            log.InfoFormat(CultureInfo.InvariantCulture, "Loading '{0}'", fileSystem.GetFullPath(this.path));

            var scriptCsLog = LogManager.GetLogger("ScriptCs");
            var engine = new RoslynScriptEngine(new ScriptHostFactory(), scriptCsLog);
            var executor = new ScriptExecutor(fileSystem, new FilePreProcessor(fileSystem, scriptCsLog), engine, scriptCsLog);

            log.DebugFormat(CultureInfo.InvariantCulture, "Initializing script executor");
            executor.Initialize(new string[0], new[] { new ConfigRScriptHack() });
            engine.BaseDirectory = fileSystem.CurrentDirectory; // NOTE (adamralph): set to bin subfolder in executor.Initialize()!

            log.DebugFormat(CultureInfo.InvariantCulture, "Compiling and executing configuration script {0}", this.path);
            var result = executor.Execute(this.path);

            log.DebugFormat(CultureInfo.InvariantCulture, "Terminating script executor");
            executor.Terminate();

            if (result.CompileException != null)
            {
                log.ErrorFormat(CultureInfo.InvariantCulture, "Error compiling configuration script {0}", result.CompileException, this.path);
                throw new InvalidOperationException("Failed to compile " + this.path, result.CompileException);
            }

            if (result.ExecuteException != null)
            {
                log.ErrorFormat(CultureInfo.InvariantCulture, "Error executing configuration script {0}", result.ExecuteException, this.path);
                throw new InvalidOperationException("Failed to execute " + this.path, result.ExecuteException);
            }

            return this;
        }
    }
}
