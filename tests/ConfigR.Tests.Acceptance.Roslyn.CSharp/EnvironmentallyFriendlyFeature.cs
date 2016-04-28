// <copyright file="EnvironmentallyFriendlyFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Tests.Acceptance
{
    using System;
    using System.IO;
    using FluentAssertions;
    using Xbehave;

    public static class EnvironmentallyFriendlyFeature
    {
        [Background]
        public static void Background()
        {
            "Given no configuration has been loaded"
                .f(() => Config.Global.Reset());
        }

        [Scenario]
        public static void CurrentDirectoryIsNotAppDirectory(
            Foo result, string currentDirectory, string originalDirectory)
        {
            "Given a local config file containing a Foo with a Bar of 'baz'"
                .f(() =>
                {
                    using (var writer = new StreamWriter(LocalScriptFileConfig.Path))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Tests.Acceptance.dll""");
                        writer.WriteLine(@"using ConfigR.Tests.Acceptance;");
                        writer.WriteLine(@"Add(""foo"", new Foo { Bar = ""baz"" });");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(LocalScriptFileConfig.Path));

            "And the current directory is not the app directory"
                .f(() =>
                {
                    originalDirectory = Environment.CurrentDirectory;
                    Environment.CurrentDirectory = currentDirectory = Path.GetTempPath();
                })
                .Teardown(() => Environment.CurrentDirectory = originalDirectory);

            "When I get the Foo"
                .f(() => result = Config.Global.Get<Foo>("foo"));

            "Then the Foo has a Bar of 'baz'"
                .f(() => result.Bar.Should().Be("baz"));

            "And the current directory is unchanged"
                .f(() => Environment.CurrentDirectory.TrimEnd('\\').Should().Be(currentDirectory.TrimEnd('\\')));
        }
    }
}
