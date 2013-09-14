﻿// <copyright file="ScriptConfigurator.cs" company="ConfigR contributors">
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

        public override IConfigurator Load()
        {
            log.DebugFormat(CultureInfo.InvariantCulture, "The current directory is {0}", this.fileSystem.CurrentDirectory);
            var path = this.GetScriptPath();
            log.DebugFormat(CultureInfo.InvariantCulture, "Executing '{0}'", this.fileSystem.GetFullPath(path));
            using (var executor = new ConfigRScriptExecutor(this.fileSystem))
            {
                executor.AddReferenceAndImportNamespaces(new[] { typeof(Configurator) });
                executor.Initialize(new string[0], new IScriptPack[0]);
                executor.Execute(path);
            }

            return this;
        }

        protected abstract string GetScriptPath();
    }
}
