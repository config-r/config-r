// <copyright file="FileConfigurator.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System.Globalization;
    using Common.Logging;
    using ScriptCs;

    public class FileConfigurator : BasicConfigurator
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();

        private readonly IFileSystem fileSystem = new ConfigRFileSystem(new FileSystem());
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
            log.DebugFormat(CultureInfo.InvariantCulture, "The current directory is {0}", this.fileSystem.CurrentDirectory);
            log.InfoFormat(CultureInfo.InvariantCulture, "Loading '{0}'", this.fileSystem.GetFullPath(this.path));

            using (var executor = new ConfigRScriptExecutor(this.fileSystem))
            {
                executor.Initialize(new string[0], new[] { new ConfigRScriptHack() });
                executor.Execute(this.path);
            }

            return this;
        }
    }
}
