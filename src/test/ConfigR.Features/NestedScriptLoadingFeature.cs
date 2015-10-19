// <copyright file="NestedScriptLoadingFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Features
{
    using System.IO;
    using FluentAssertions;
    using Xbehave;

    public static class NestedScriptLoadingFeature
    {
        [Background]
        public static void Background()
        {
            "Given no configuration has been loaded"
                .f(() => Config.Global.Reset());
        }

        [Scenario]
        public static void UsingStatementsFollowingLoadingOfAScriptContainingCode(Foo result)
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

            "And another config file which loads the first config file as a script and then has using statements"
                .f(() =>
                {
                    using (var writer = new StreamWriter("bar.csx"))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"#load ""foo.csx""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete("bar.csx"));

            "When I load the second config file"
                .f(() => Config.Global.LoadScriptFile("bar.csx"));

            "And I get the Foo"
                .f(() => result = Config.Global.Get<Foo>("foo"));

            "Then the Foo has a Bar of 'baz'"
                .f(() => result.Bar.Should().Be("baz"));
        }
    }
}
