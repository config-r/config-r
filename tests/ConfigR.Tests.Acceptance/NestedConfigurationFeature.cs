// <copyright file="NestedConfigurationFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Tests.Acceptance
{
    using System;
    using System.IO;
    using FluentAssertions;
    using Xbehave;

    public static class NestedConfigurationFeature
    {
        [Background]
        public static void Background()
        {
            "Given no configuration has been loaded"
                .f(() => Config.Global.Reset());
        }

        [Scenario]
        public static void RetrievingAnObjectFromANestedFile(Foo result)
        {
            "Given a config file containing a Foo with a Bar of 'baz'"
                .f(() =>
                {
                    using (var writer = new StreamWriter("foo.csx"))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Tests.Acceptance.dll""");
                        writer.WriteLine(@"using ConfigR.Tests.Acceptance;");
                        writer.WriteLine(@"Add(""foo"", new Foo { Bar = ""baz"" });");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete("foo.csx"));

            "And another config file which loads the first config file"
                .f(() =>
                {
                    using (var writer = new StreamWriter("bar.csx"))
                    {
                        writer.WriteLine(@"LoadScriptFile(""foo.csx"");");
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

        [Scenario]
        public static void ExceptionThrownAfterExecutionOfNestedConfiguration()
        {
            "Given a config file containing a Foo with a Bar of 'baz'"
                .f(() =>
                {
                    using (var writer = new StreamWriter("foo.csx"))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Tests.Acceptance.dll""");
                        writer.WriteLine(@"using ConfigR.Tests.Acceptance;");
                        writer.WriteLine(@"Add(""foo"", new Foo { Bar = ""baz"" });");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete("foo.csx"));

            "And another config file which loads the first config file and then throws an exception"
                .f(() =>
                {
                    using (var writer = new StreamWriter("bar.csx"))
                    {
                        writer.WriteLine(@"LoadScriptFile(""foo.csx"");");
                        writer.WriteLine(@"throw new InvalidOperationException();");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete("bar.csx"));

            "When I load the second config file"
                .f(() =>
                {
                    try
                    {
                        Config.Global.LoadScriptFile("bar.csx");
                    }
                    catch (InvalidOperationException)
                    {
                    }
                });

            "Then the Foo is not available"
                .f(() => Config.Global.Should().NotContainKey("foo"));
        }
    }
}
