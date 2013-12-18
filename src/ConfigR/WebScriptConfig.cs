// <copyright file="WebScriptConfig.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using Common.Logging;
    using ConfigR.Scripting;

    public class WebScriptConfig : BasicConfig
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();
        private readonly Uri uri;

        public WebScriptConfig(Uri uri)
        {
            Guard.AgainstNullArgument("uri", uri);

            this.uri = uri;
        }

        public Uri Uri
        {
            get { return this.uri; }
        }

        protected override string Source
        {
            get { return this.uri.ToString(); }
        }

        public override ISimpleConfig Load()
        {
            var path = Path.GetTempFileName();
            log.DebugFormat(CultureInfo.InvariantCulture, "Downloading '{0}' to '{1}'.", this.uri.ToString(), path);
            
            var request = WebRequest.Create(this.uri);
            using (var response = request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            using (var fileStream = File.OpenWrite(path))
            {
                responseStream.CopyTo(fileStream);
            }

            new ScriptConfigLoader().LoadFromFile(this, path);
            return this;
        }
    }
}
