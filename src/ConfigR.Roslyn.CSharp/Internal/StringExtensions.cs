// <copyright file="StringExtensions.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Roslyn.CSharp.Internal
{
    using System;
    using System.IO;
    using System.Net;
    using ConfigR.Roslyn.CSharp.Logging;
    using ConfigR.Sdk;
    using static System.FormattableString;

    public static class StringExtensions
    {
        private static readonly ILog log = LogProvider.GetCurrentClassLogger();

        public static Uri ResolveScriptUri(this string address)
        {
            address = address ??
                Path.ChangeExtension(AppDomain.CurrentDomain.SetupInformation.VSHostingAgnosticConfigurationFile(), "csx");

            if (address == null)
            {
                throw new InvalidOperationException("AppDomain.CurrentDomain.SetupInformation.ConfigurationFile is null.");
            }

            Uri uri;
            if (!Uri.TryCreate(address, UriKind.Absolute, out uri))
            {
                // NOTE (adamralph): ApplicationBase *should* be absolute so Path.GetFullPath() *should* be redundant.
                return new Uri(
                    Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, address)));
            }

            return uri;
        }

        public static string ToFilePath(this Uri uri)
        {
            string path;
            if (uri.TryGetFilePath(out path))
            {
                return path;
            }

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

        public static bool TryGetFilePath(this Uri uri, out string path)
        {
            return (path = uri.Scheme == Uri.UriSchemeFile
                ? uri.LocalPath + Uri.UnescapeDataString(uri.Fragment).Replace('/', Path.DirectorySeparatorChar)
                : default(string)) != null;
        }
    }
}
