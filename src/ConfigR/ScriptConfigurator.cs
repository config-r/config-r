// <copyright file="ScriptConfigurator.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.Globalization;
    using Common.Logging;
    using ScriptCs;
    using ScriptCs.Contracts;

    public abstract class ScriptConfigurator : BasicConfigurator
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();

        private readonly IFileSystem fileSystem = new FileSystem { CurrentDirectory = AppDomain.CurrentDomain.SetupInformation.ApplicationBase };

        protected abstract string ScriptPath { get; }

        public override IConfigurator Load()
        {
            log.DebugFormat(CultureInfo.InvariantCulture, "The current directory is {0}", this.fileSystem.CurrentDirectory);
            log.DebugFormat(CultureInfo.InvariantCulture, "Executing '{0}'", this.fileSystem.GetFullPath(this.ScriptPath));
            using (var executor = new ConfigRScriptExecutor(this, this.fileSystem))
            {
                try
                {
                    executor.AddReferenceAndImportNamespaces(new[] { typeof(Configurator) });
                    executor.Initialize(new string[0], new IScriptPack[0]);
                    executor.Execute(path);
                }
                catch (Roslyn.Compilers.CompilationErrorException ex)
                {
                    log.DebugFormat(CultureInfo.InvariantCulture, "Compilation exception caught - " + ex.ToString());
                    var exception = new Exception("Compilation exception occurred", ex);
                    exception.Data.Add("Error_Code", 111);
                    throw exception;
                }
            }

            return this;
        }
    }
}
