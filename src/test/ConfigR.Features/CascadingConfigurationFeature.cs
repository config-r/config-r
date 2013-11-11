// <copyright file="CascadingConfigurationFeature.cs" company="ConfigR contributors">
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

    public static class CascadingConfigurationFeature
    {
        [Background]
        public static void Background()
        {
            "Given no configuration is loaded"
                .Given(() => Configurator.Unload());
        }

        [Scenario]
        public static void RetreivingAnObjectDefinedInTwoFiles(Foo result)
        {
            "Given a config file containing a Foo with a Bar of 'baz'"
                .Given(() =>
                {
                    using (var writer = new StreamWriter("foo1.csx"))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Add(""foo"", new CascadingConfigurationFeature.Foo { Bar = ""baz"" });");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete("foo1.csx"));

            "And another config file containing a Foo with a Bar of 'bazzzzz'"
                .Given(() =>
                {
                    using (var writer = new StreamWriter("foo2.csx"))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Add(""foo"", new CascadingConfigurationFeature.Foo { Bar = ""bazzzzz"" });");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete("foo2.csx"));

            "When I load the first file"
                .When(() => Configurator.Load("foo1.csx"));

            "And I load the second file"
                .And(() => Configurator.Load("foo2.csx"));

            "And I get the Foo"
                .And(() => result = Configurator.Get<Foo>("foo"));

            "Then the Foo has a Bar of 'baz'"
                .Then(() => result.Bar.Should().Be("baz"));
        }

        [Scenario]
        public static void RetreivingAnObjectDefinedInTheSecondOfTwoFiles(Foo result)
        {
            "Given a config file not containing a Foo"
                .Given(() =>
                {
                    using (var writer = new StreamWriter("foo1.csx"))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Add(""notfoo"", new CascadingConfigurationFeature.Foo { Bar = ""baz"" });");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete("foo1.csx"));

            "And another config file containing a Foo with a Bar of 'bazzzzz'"
                .Given(() =>
                {
                    using (var writer = new StreamWriter("foo2.csx"))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Add(""foo"", new CascadingConfigurationFeature.Foo { Bar = ""bazzzzz"" });");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete("foo2.csx"));

            "When I load the first file"
                .When(() => Configurator.Load("foo1.csx"));

            "And I load the second file"
                .And(() => Configurator.Load("foo2.csx"));

            "And I get the Foo"
                .And(() => result = Configurator.Get<Foo>("foo"));

            "Then the Foo has a Bar of 'baz'"
                .Then(() => result.Bar.Should().Be("bazzzzz"));
        }

        [Scenario]
        public static void RetreivingAnObjectDefinedInAFileWhoseNameIsDefinedInAnotherFile(string otherFileName, Foo result)
        {
            "Given a config file containing the name of another config file"
                .Given(() =>
                {
                    using (var writer = new StreamWriter("foo1.csx"))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Add(""otherFileName"", ""foo2.csx"");");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete("foo1.csx"));

            "And another config file containing a Foo with a Bar of 'bazzzzz'"
                .Given(() =>
                {
                    using (var writer = new StreamWriter("foo2.csx"))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Add(""foo"", new CascadingConfigurationFeature.Foo { Bar = ""bazzzzz"" });");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete("foo2.csx"));

            "When I load the first file"
                .When(() => Configurator.Load("foo1.csx"));

            "And I get the name of the other file"
                .And(() => otherFileName = Configurator.Get<string>("otherFileName"));

            "And I load the second file"
                .And(() => Configurator.Load(otherFileName));

            "And I get the Foo"
                .And(() => result = Configurator.Get<Foo>("foo"));

            "Then the Foo has a Bar of 'baz'"
                .Then(() => result.Bar.Should().Be("bazzzzz"));
        }

        [Scenario]
        public static void TryingToRetreiveANonExistentObject(Exception ex)
        {
            "Given an empty config file"
                .f(() =>
                {
                    using (var writer = new StreamWriter("foo1.csx"))
                    {
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete("foo1.csx"));

            "When I load the file"
                .f(() => Configurator.Load("foo1.csx"));

            "And I try and get an object named 'foo'"
                .f(() => ex = Record.Exception(() => Configurator.Get<Foo>("foo")));

            "Then an exception is thrown"
                .f(() => ex.Should().NotBeNull());

            "And the exception message contains 'foo'"
                .f(() => ex.Message.Should().Contain("foo"));
        }

        public class Foo
        {
            public string Bar { get; set; }
        }
    }
}
