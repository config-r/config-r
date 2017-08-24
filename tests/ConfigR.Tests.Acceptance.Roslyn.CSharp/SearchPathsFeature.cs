// <copyright file="SearchPathsFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Tests.Acceptance
{
    using System.IO;
    using ConfigR.Tests.Acceptance.Roslyn.CSharp.Support;
    using FluentAssertions;
    using Xbehave;
    using Foo = ConfigR.Tests.Support.SampleDependency.Foo;

    public static class SearchPathsFeature
    {
        [Scenario]
        public static void LoadingAScriptFromTheScriptFolder(string path1, string path2, int foo)
        {
            dynamic config = null;

            "Given a remote config file with a Foo of 123"
                .x(c => ConfigFile.Create(
                        "Config.Foo = 123;",
                        path1 = Path.Combine(Path.GetTempPath(), Path.GetTempFileName()))
                    .Using(c));

            "And another remote config file in the same folder, which loads the first file"
                .x(c => ConfigFile.Create(
                        $@"#load ""{Path.GetFileName(path1)}""",
                        path2 = Path.Combine(Path.GetTempPath(), Path.GetTempFileName()))
                    .Using(c));

            "When I load the second config file"
                .x(async () => config = await new Config().UseRoslynCSharpLoader(path2).LoadDynamic());

            "And I get Foo"
                .x(() => foo = config.Foo<int>());

            "Then Foo is 123"
                .x(() => foo.Should().Be(123));
        }

        [Scenario]
        public static void LoadingAScriptFromTheApplicationFolder(string path1, string path2, int foo)
        {
            dynamic config = null;

            "Given a local config file with a Foo of 123"
                .x(c => ConfigFile.Create("Config.Foo = 123;", path1 = "foo.csx").Using(c));

            "And remote config file which loads the first file"
                .x(c => ConfigFile.Create(
                        $@"#load ""foo.csx""",
                        path2 = Path.Combine(Path.GetTempPath(), Path.GetTempFileName()))
                    .Using(c));

            "When I load the second config file"
                .x(async () => config = await new Config().UseRoslynCSharpLoader(path2).LoadDynamic());

            "And I get Foo"
                .x(() => foo = config.Foo<int>());

            "Then Foo is 123"
                .x(() => foo.Should().Be(123));
        }

        [Scenario]
        public static void LoadingAScriptFromWeb(string foo)
        {
            dynamic config = null;
            
            "Given remote config file which loads the first file"
                .x(c => ConfigFile.Create(
                        $@"#load ""https://gist.githubusercontent.com/adamralph/9c4d6a6a705e1762646fbcf124f634f9/raw/d15f7331621e9065c566e94f32972546711ef29a/sample-config3.csx""")
                    .Using(c));

            "When I load the config from web"
                .x(async () => config = await new Config().UseRoslynCSharpLoader().LoadDynamic());

            "And I get WebGreeting"
                .x(() => foo = config.WebGreeting<string>());

            "Then Foo is 123"
                .x(() => foo.Should().Be("Hello World from web!"));
        }


        [Scenario]
        public static void ReferencingAnAssemblyFromTheScriptFolder(string path1, string path2, Foo foo)
        {
            dynamic config = null;

            "Given a remote assembly"
                .x(c => File.Copy(
                    "ConfigR.Tests.Support.SampleDependency.dll",
                    path1 = Path.Combine(Path.GetTempPath(), Path.ChangeExtension(Path.GetTempFileName(), "dll")),
                    true));

            "And a remote config file in the same folder, which references the assembly"
                .x(c =>
                {
                    var code =
$@"#r ""{Path.GetFileName(path1)}""
using ConfigR.Tests.Support.SampleDependency;
Config.Foo = new Foo {{ Bar = ""baz"" }};
";

                    ConfigFile.Create(code, path2 = Path.Combine(Path.GetTempPath(), Path.GetTempFileName())).Using(c);
                });

            "When I load the config file"
                .x(async () => config = await new Config().UseRoslynCSharpLoader(path2).LoadDynamic());

            "And I get Foo"
                .x(() => foo = config.Foo<Foo>());

            "Then Foo has a Bar of 'baz'"
                .x(() => foo.Bar.Should().Be("baz"));
        }

        [Scenario]
        public static void ReferencingAnAssemblyFromTheApplicationFolder(string path, Foo foo)
        {
            dynamic config = null;

            "Given a remote config file which references a local assembly"
                .x(c =>
                {
                    var code =
$@"#r ""ConfigR.Tests.Support.SampleDependency.dll""
using ConfigR.Tests.Support.SampleDependency;
Config.Foo = new Foo {{ Bar = ""baz"" }};
";
                    var remoteAssemblyPath = Path.Combine(Path.GetTempPath(), "ConfigR.Tests.Support.SampleDependency.dll");
                    if (File.Exists(remoteAssemblyPath))
                    {
                        File.Delete(remoteAssemblyPath);
                    }

                    ConfigFile.Create(code, path = Path.Combine(Path.GetTempPath(), Path.GetTempFileName())).Using(c);
                });

            "When I load the config file"
                .x(async () => config = await new Config().UseRoslynCSharpLoader(path).LoadDynamic());

            "And I get Foo"
                .x(() => foo = config.Foo<Foo>());

            "Then Foo has a Bar of 'baz'"
                .x(() => foo.Bar.Should().Be("baz"));
        }
    }
}
