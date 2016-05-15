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
                .f(c => ConfigFile.Create(
                        "Config.Foo = 123;",
                        path1 = Path.Combine(Path.GetTempPath(), Path.GetTempFileName()))
                    .Using(c));

            "And another remote config file in the same folder, which loads the first file"
                .f(c => ConfigFile.Create(
                        $@"#load ""{Path.GetFileName(path1)}""",
                        path2 = Path.Combine(Path.GetTempPath(), Path.GetTempFileName()))
                    .Using(c));

            "When I load the second config file"
                .f(async () => config = await new Config().UseRoslynCSharpLoader(path2).Load());

            "And I get Foo"
                .f(() => foo = config.Foo<int>());

            "Then Foo is 123"
                .f(() => foo.Should().Be(123));
        }

        [Scenario]
        public static void LoadingAScriptFromTheApplicationFolder(string path1, string path2, int foo)
        {
            dynamic config = null;

            "Given a local config file with a Foo of 123"
                .f(c => ConfigFile.Create("Config.Foo = 123;", path1 = "foo.csx").Using(c));

            "And remote config file which loads the first file"
                .f(c => ConfigFile.Create(
                        $@"#load ""foo.csx""",
                        path2 = Path.Combine(Path.GetTempPath(), Path.GetTempFileName()))
                    .Using(c));

            "When I load the second config file"
                .f(async () => config = await new Config().UseRoslynCSharpLoader(path2).Load());

            "And I get Foo"
                .f(() => foo = config.Foo<int>());

            "Then Foo is 123"
                .f(() => foo.Should().Be(123));
        }

        [Scenario]
        public static void ReferencingAnAssemblyFromTheScriptFolder(string path1, string path2, Foo foo)
        {
            dynamic config = null;

            "Given a remote assembly"
                .f(c => File.Copy(
                    "ConfigR.Tests.Support.SampleDependency.dll",
                    path1 = Path.Combine(Path.GetTempPath(), Path.ChangeExtension(Path.GetTempFileName(), "dll")),
                    true));

            "And a remote config file in the same folder, which references the assembly"
                .f(c =>
                {
                    var code =
$@"#r ""{Path.GetFileName(path1)}""
using ConfigR.Tests.Support.SampleDependency;
Config.Foo = new Foo {{ Bar = ""baz"" }};
";

                    ConfigFile.Create(code, path2 = Path.Combine(Path.GetTempPath(), Path.GetTempFileName())).Using(c);
                });

            "When I load the config file"
                .f(async () => config = await new Config().UseRoslynCSharpLoader(path2).Load());

            "And I get Foo"
                .f(() => foo = config.Foo<Foo>());

            "Then Foo has a Bar of 'baz'"
                .f(() => foo.Bar.Should().Be("baz"));
        }

        [Scenario]
        public static void ReferencingAnAssemblyFromTheApplicationFolder(string path1, string path2, Foo foo)
        {
            dynamic config = null;

            "Given a remote config file which references a local assembly"
                .f(c =>
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

                    ConfigFile.Create(code, path2 = Path.Combine(Path.GetTempPath(), Path.GetTempFileName())).Using(c);
                });

            "When I load the config file"
                .f(async () => config = await new Config().UseRoslynCSharpLoader(path2).Load());

            "And I get Foo"
                .f(() => foo = config.Foo<Foo>());

            "Then Foo has a Bar of 'baz'"
                .f(() => foo.Bar.Should().Be("baz"));
        }
    }
}
