// <copyright file="FileConfigurator.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using Common.Logging;

    public class FileConfigurator : ScriptConfigurator
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
            log.InfoFormat("Loading '{0}'", this.path);
            return base.Load();
        }

        protected override string GetScriptPath()
        {
            return this.path;
        }
    }
}
