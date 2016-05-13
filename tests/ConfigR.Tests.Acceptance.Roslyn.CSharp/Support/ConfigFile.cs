// <copyright file="ConfigFile.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Tests.Acceptance.Roslyn.CSharp.Support
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    public static class ConfigFile
    {
        [SuppressMessage(
            "StyleCop.CSharp.LayoutRules",
            "SA1500:CurlyBracketsForMultiLineStatementsMustNotShareLine",
            Justification = "Bug in StyleCop - see https://stylecop.codeplex.com/workitem/7723.")]
        public static string DefaultPath { get; } =
            Path.ChangeExtension(AppDomain.CurrentDomain.SetupInformation.VSHostingAgnosticConfigurationFile(), "csx");

        public static IDisposable Create(string contents)
        {
            using (var writer = new StreamWriter(DefaultPath))
            {
                writer.Write(contents);
                writer.Flush();
            }

            return new Disposable(() => File.Delete(DefaultPath));
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
