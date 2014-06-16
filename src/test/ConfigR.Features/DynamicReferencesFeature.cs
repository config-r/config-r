// <copyright file="DynamicReferencesFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Features
{
    using System;
    using System.IO;
    using System.Reflection;
    using FluentAssertions;
    using Xbehave;
    using Xunit;

    public static class DynamicReferencesFeature
    {
        [Background]
        public static void Background()
        {
            "Given no configuration has been loaded"
                .Given(() => Config.Global.Reset());
        }

        [Scenario]
        public static void AddingADynamicReferenceToAnExplcitFile(Assembly reference, object result)
        {
            "Given a config file adds a Foo from a dynamically loaded assembly with a Bar of 'baz'"
                .f(() =>
                {
                    using (var writer = new StreamWriter("foo.csx"))
                    {
                        writer.WriteLine(@"using ConfigR.Testing.External;");
                        writer.WriteLine(@"Add(""foo"", new Foo { Bar = ""baz"" });");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete("foo.csx"));

            "When I load the assembly"
                .f(() => reference = Assembly.LoadFile(Path.GetFullPath("ConfigR.Testing.External.dll")));

            "And I load the config using the assembly as a reference"
                .f(() => result = Config.Global.LoadScriptFile("foo.csx", reference));

            "And I get the value"
                .f(() => result = Config.Global.Get());

            "Then the Foo Bar should be 'baz'"
                .f(() => ((string)((dynamic)result).Bar).Should().Be("baz"));
        }

        [Scenario]
        public static void AddingADynamicReferenceToALocalFile(Assembly reference, object result)
        {
            "Given a local config file adds a Foo from a dynamically loaded assembly with a Bar of 'baz'"
                .f(() =>
                {
                    using (var writer = new StreamWriter(LocalScriptFileConfig.Path))
                    {
                        writer.WriteLine(@"using ConfigR.Testing.External;");
                        writer.WriteLine(@"Add(""foo"", new Foo { Bar = ""baz"" });");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(LocalScriptFileConfig.Path));

            "When I load the assembly"
                .f(() => reference = Assembly.LoadFile(Path.GetFullPath("ConfigR.Testing.External.dll")));

            "When I disable global auto loading"
                .f(() => Config.DisableGlobalAutoLoading())
                .Teardown(() => Config.EnableGlobalAutoLoading());

            "And I load the config using the assembly as a reference"
                .f(() => result = Config.Global.LoadLocalScriptFile(reference));

            "And I get the value"
                .f(() => result = Config.Global.Get());

            "Then the Foo Bar should be 'baz'"
                .f(() => ((string)((dynamic)result).Bar).Should().Be("baz"));
        }

        [Scenario]
        public static void AddingADynamicReferenceToGlobalAutoLoading(Assembly reference, object result)
        {
            "Given a local config file adds a Foo from a dynamically loaded assembly with a Bar of 'baz'"
                .f(() =>
                {
                    using (var writer = new StreamWriter(LocalScriptFileConfig.Path))
                    {
                        writer.WriteLine(@"using ConfigR.Testing.External;");
                        writer.WriteLine(@"Add(""foo"", new Foo { Bar = ""baz"" });");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(LocalScriptFileConfig.Path));

            "When I load the assembly"
                .f(() => reference = Assembly.LoadFile(Path.GetFullPath("ConfigR.Testing.External.dll")));

            "And I add the reference to global autoloading"
                .f(() => Config.GlobalAutoLoadingReferences.Add(reference));

            "And I get the value"
                .f(() => result = Config.Global.Get());

            "Then the Foo Bar should be 'baz'"
                .f(() => ((string)((dynamic)result).Bar).Should().Be("baz"));
        }
    }
}
