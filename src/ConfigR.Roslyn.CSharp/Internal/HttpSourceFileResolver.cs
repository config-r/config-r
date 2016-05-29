// <copyright file="HttpSourceFileResolver.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Roslyn.CSharp.Internal
{
    using System;
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;

    [CLSCompliant(false)]
    public class HttpSourceFileResolver : SourceFileResolver, IEquatable<HttpSourceFileResolver>
    {
        [CLSCompliant(false)]
        public HttpSourceFileResolver(ImmutableArray<string> searchPaths, string baseDirectory)
            : base(searchPaths, baseDirectory)
        {
        }

        public override string ResolveReference(string path, string baseFilePath)
        {
            Uri uri;
            return base.ResolveReference(
                Uri.TryCreate(path, UriKind.Absolute, out uri) ? uri.ToFilePath() : path, baseFilePath);
        }

        public bool Equals(HttpSourceFileResolver other) => base.Equals(other);

        public override int GetHashCode() => base.GetHashCode();

        public override bool Equals(object obj) => base.Equals(obj);
    }
}
