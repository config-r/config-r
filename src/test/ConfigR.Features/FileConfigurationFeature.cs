// <copyright file="FileConfigurationFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Features
{
    using System.IO;
    using FluentAssertions;
    using Xbehave;

    public static class FileConfigurationFeature
    {
        [Background]
        public static void Background()
        {
            "Given no configuration has been loaded"
                .f(() => Config.Global.Reset());
        }

        [Scenario]
        public static void RetrievingAnObject(Foo result)
        {
            "Given a config file containing a Foo with a Bar of 'baz'"
                .f(() =>
                {
                    using (var writer = new StreamWriter("foo.csx"))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Add(""foo"", new Foo { Bar = ""baz"" });");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete("foo.csx"));

            "When I load the file"
                .f(() => Config.Global.LoadScriptFile("foo.csx"));

            "And I get the Foo"
                .f(() => result = Config.Global.Get<Foo>("foo"));

            "Then the Foo has a Bar of 'baz'"
                .f(() => result.Bar.Should().Be("baz"));
        }
    }
}
