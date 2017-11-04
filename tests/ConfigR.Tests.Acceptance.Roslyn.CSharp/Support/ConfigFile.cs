// <copyright file="ConfigFile.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Tests.Acceptance.Roslyn.CSharp.Support
{
    using System;
    using System.IO;

    public static class ConfigFile
    {
        private static readonly string defaultPath = Path.ChangeExtension(GetDefaultPath(), "csx");

        public static IDisposable Create(string contents) => Create(contents, defaultPath);

        public static IDisposable Create(string contents, string path)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.Write(contents);
                writer.Flush();
            }

            return new Disposable(() => File.Delete(path));
        }

        public static string GetDefaultPath()
        {
            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", "Test.exe.config");
            return AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
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
