// <copyright file="ConfigFile.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Tests.Acceptance.Roslyn.CSharp.Support
{
    using System;
    using System.IO;

    public static class ConfigFile
    {
        public static string DefaultPath { get; } = GetDefaultPath();

        public static IDisposable Create(string contents)
        {
            using (var writer = new StreamWriter(DefaultPath))
            {
                writer.Write(contents);
                writer.Flush();
            }

            return new Disposable(() => File.Delete(DefaultPath));
        }

        private static string GetDefaultPath()
        {
            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", "Test.config");
            return Path.ChangeExtension(
                AppDomain.CurrentDomain.SetupInformation.VSHostingAgnosticConfigurationFile(), "csx");
        }

        private sealed class Disposable : IDisposable
        {
            private readonly Action whenDisposed;

            public Disposable(Action whenDisposed)
            {
                this.whenDisposed = whenDisposed;
            }

            public void Dispose() => this.whenDisposed?.Invoke();
        }
    }
}
