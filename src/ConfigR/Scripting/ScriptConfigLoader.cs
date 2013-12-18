// <copyright file="ScriptConfigLoader.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Scripting
{
    using System;
    using System.Globalization;
    using Common.Logging;
    using ScriptCs;
    using ScriptCs.Contracts;

    public class ScriptConfigLoader
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();

        private readonly IFileSystem fileSystem =
            new FileSystem { CurrentDirectory = AppDomain.CurrentDomain.SetupInformation.ApplicationBase };

        public object LoadFromFile(ISimpleConfig config, string path)
        {
            log.InfoFormat(CultureInfo.InvariantCulture, "Executing '{0}'", this.fileSystem.GetFullPath(path));
            log.DebugFormat(CultureInfo.InvariantCulture, "The current directory is {0}", this.fileSystem.CurrentDirectory);
            using (var executor = new ConfigRScriptExecutor(config, this.fileSystem))
            {
                executor.AddReferenceAndImportNamespaces(new[] { typeof(Config) });
                executor.Initialize(new string[0], new IScriptPack[0]);
                return executor.Execute(path).ReturnValue;
            }
        }
    }
}
