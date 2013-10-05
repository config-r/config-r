// <copyright file="WebConfigurator.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using Common.Logging;

    public class WebConfigurator : ScriptConfigurator
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();
        private readonly Uri uri;
        private string scriptPath;

        public WebConfigurator(Uri uri)
        {
            Guard.AgainstNullArgument("uri", uri);

            this.uri = uri;
        }

        public Uri Uri
        {
            get { return this.uri; }
        }

        protected override string ScriptPath
        {
            get { return this.scriptPath; }
        }

        public override IConfigurator Load()
        {
            log.InfoFormat(CultureInfo.InvariantCulture, "Loading '{0}'", this.uri.ToString());

            var path = Path.GetTempFileName();
            var request = WebRequest.Create(this.uri);

            log.DebugFormat(CultureInfo.InvariantCulture, "Downloading script from {0}", this.uri.ToString());
            using (var response = request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            using (var fileStream = File.OpenWrite(path))
            {
                log.DebugFormat(CultureInfo.InvariantCulture, "Writing to temporary script file {0}", path);
                responseStream.CopyTo(fileStream);
            }

            this.scriptPath = path;
            return base.Load();
        }
    }
}
