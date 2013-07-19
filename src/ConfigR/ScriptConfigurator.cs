// <copyright file="ScriptConfigurator.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System.Globalization;
    using Common.Logging;
    using ScriptCs;

    public abstract class ScriptConfigurator : BasicConfigurator
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();

        private readonly IFileSystem fileSystem = new ConfigRFileSystem(new FileSystem());

        public override IConfigurator Load()
        {
            log.DebugFormat(CultureInfo.InvariantCulture, "The current directory is {0}", this.fileSystem.CurrentDirectory);
            var path = this.GetScriptPath();
            log.InfoFormat(CultureInfo.InvariantCulture, "Loading '{0}'", this.fileSystem.GetFullPath(path));
            using (var executor = new ConfigRScriptExecutor(this.fileSystem))
            {
                executor.Initialize(new string[0], new[] { new ConfigRScriptHack() });
                executor.Execute(path);
            }

            return this;
        }

        protected abstract string GetScriptPath();
    }
}
