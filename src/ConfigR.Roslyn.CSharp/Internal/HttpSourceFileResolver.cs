// <copyright file="HttpSourceFileResolver.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Roslyn.CSharp.Internal
{
    using System;
    using System.Collections.Immutable;
    using System.IO;
    using System.Net;
    using ConfigR.Roslyn.CSharp.Logging;
    using Microsoft.CodeAnalysis;
    using static System.FormattableString;

    [CLSCompliant(false)]
    public class HttpSourceFileResolver : SourceFileResolver, IEquatable<HttpSourceFileResolver>
    {
        private static readonly ILog log = LogProvider.GetCurrentClassLogger();

        [CLSCompliant(false)]
        public HttpSourceFileResolver(ImmutableArray<string> searchPaths, string baseDirectory)
            : base(searchPaths, baseDirectory)
        {
        }

        public override string ResolveReference(string path, string baseFilePath)
        {
            Uri uri;
            if (Uri.TryCreate(path, UriKind.Absolute, out uri) && (uri.Scheme == "http" || uri.Scheme == "https"))
            {
                path = Path.GetTempFileName();
                log.InfoFormat("Downloading '{0}' to '{1}'.", uri.ToString(), path);

                using (var response = WebRequest.Create(uri).GetResponse())
                using (var responseStream = response.GetResponseStream())
                using (var fileStream = File.OpenWrite(path))
                {
                    if (responseStream == null)
                    {
                        throw new InvalidOperationException(Invariant($"No response received from '{uri}'."));
                    }

                    responseStream.CopyTo(fileStream);
                }

                return path;
            }

            return base.ResolveReference(path, baseFilePath);
        }

        public bool Equals(HttpSourceFileResolver other) => base.Equals(other);

        public override int GetHashCode() => base.GetHashCode();

        public override bool Equals(object obj) => base.Equals(obj);
    }
}
