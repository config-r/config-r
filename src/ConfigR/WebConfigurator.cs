// <copyright file="WebConfigurator.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.IO;
    using System.Net;
    using Common.Logging;

    public class WebConfigurator : ScriptConfigurator
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();

        private readonly Uri uri;

        public WebConfigurator(Uri uri)
        {
            Guard.AgainstNullArgument("uri", uri);

            this.uri = uri;
        }

        public Uri Uri
        {
            get { return this.uri; }
        }

        protected override string GetScriptPath()
        {
            var path = Path.GetTempFileName();
            var request = WebRequest.Create(this.uri);

            log.DebugFormat("Downloading script from {0}", this.uri.ToString());
            using (var response = request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            using (var fileStream = File.OpenWrite(path))
            {
                log.DebugFormat("Writing to temporary script file {0}", path);
                responseStream.CopyTo(fileStream);
            }

            return path;
        }
    }
}
