// <copyright file="NestedConfigurationFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Features
{
    using System.IO;
    using FluentAssertions;
    using Xbehave;

    public static class NestedConfigurationFeature
    {
        [Background]
        public static void Background()
        {
            "Given no configuration is loaded"
                .Given(() => Configurator.Unload());
        }

        [Scenario]
        public static void RetreivingAnObjectFromANestedFile(Foo result)
        {
            "Given a config file containing a Foo with a Bar of 'baz'"
                .Given(() =>
                {
                    using (var writer = new StreamWriter("foo.csx"))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Add(""foo"", new NestedConfigurationFeature.Foo { Bar = ""baz"" });");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete("foo.csx"));

            "And another config file which loads the first config file"
                .And(() =>
                {
                    using (var writer = new StreamWriter("bar.csx"))
                    {
                        writer.WriteLine(@"Load(""foo.csx"");");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete("bar.csx"));

            "When I load the second config file"
                .When(() => Configurator.Load("bar.csx"));

            "And I get the Foo"
                .And(() => result = Configurator.Get<Foo>("foo"));

            "Then the Foo has a Bar of 'baz'"
                .Then(() => result.Bar.Should().Be("baz"));
        }

        [Scenario]
        public static void ExceptionThrownAfterExecutionOfNestedConfiguration()
        {
            "Given a config file containing a Foo with a Bar of 'baz'"
                .Given(() =>
                {
                    using (var writer = new StreamWriter("foo.csx"))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Add(""foo"", new NestedConfigurationFeature.Foo { Bar = ""baz"" });");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete("foo.csx"));

            "And another config file which loads the first config file and then throws an exception"
                .And(() =>
                {
                    using (var writer = new StreamWriter("bar.csx"))
                    {
                        writer.WriteLine(@"Load(""foo.csx"");");
                        writer.WriteLine(@"throw new Exception();");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete("bar.csx"));

            "When I load the second config file"
                .When(() =>
                {
                    try
                    {
                        Configurator.Load("bar.csx");
                    }
                    catch
                    {
                    }
                });

            "Then the Foo is not available"
                .Then(() => Configurator.Items.Should().NotContain(pair => pair.Key == "foo"));
        }

        public class Foo
        {
            public string Bar { get; set; }
        }
    }
}
