// <copyright file="HttpSourceFileResolver.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Roslyn.CSharp.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.IO;
    using System.Net.Http;
    using System.Text;
    using Microsoft.CodeAnalysis;

    [CLSCompliant(false)]
    public class HttpSourceFileResolver : SourceFileResolver, IEquatable<HttpSourceFileResolver>
    {
        private readonly Dictionary<string, string> _remoteFiles = new Dictionary<string, string>();

        [CLSCompliant(false)]
        public HttpSourceFileResolver(ImmutableArray<string> searchPaths, string baseDirectory)
            : base(searchPaths, baseDirectory)
        {
        }

        public bool Equals(HttpSourceFileResolver other)
        {
            var result = base.Equals(other);
            return other == null ? result : result && Equals(_remoteFiles, other._remoteFiles);
        }

        public override string ResolveReference(string path, string baseFilePath)
        {
            var uri = GetUri(path);
            if (uri != null)
            {
                var client = new HttpClient();
                var response = client.GetAsync(path).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseFile = response.Content.ReadAsStringAsync().Result;
                    if (!string.IsNullOrWhiteSpace(responseFile))
                    {
                        _remoteFiles.Add(path, responseFile);
                        return path;
                    }
                }
            }

            return base.ResolveReference(path, baseFilePath);
        }

        public override Stream OpenRead(string resolvedPath)
        {
            var uri = GetUri(resolvedPath);
            if ((uri != null) && _remoteFiles.ContainsKey(resolvedPath))
            {
                var storedFile = _remoteFiles[resolvedPath];
                return new MemoryStream(Encoding.UTF8.GetBytes(storedFile));
            }

            return base.OpenRead(resolvedPath);
        }

        public override string NormalizePath(string path, string baseFilePath)
        {
            var uri = GetUri(path);
            if (uri == null)
                return base.NormalizePath(path, baseFilePath);

            return path;
        }

        private static Uri GetUri(string input)
        {
            Uri uriResult;
            if (Uri.TryCreate(input, UriKind.Absolute, out uriResult)
                && ((uriResult.Scheme == "http")
                    || (uriResult.Scheme == "https")))
                return uriResult;

            return null;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 37;
                hashCode = (hashCode*397) ^ (_remoteFiles?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ base.GetHashCode();
                return hashCode;
            }
        }

        public override bool Equals(object obj) => base.Equals(obj);
    }
}