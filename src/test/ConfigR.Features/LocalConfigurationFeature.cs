// <copyright file="LocalConfigurationFeature.cs" company="ConfigR contributors">
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

    public static class LocalConfigurationFeature
    {
        [Scenario]
        public static void RetreivingAnObject(Foo result)
        {
            "Given a local config file containing a Foo with a Bar of 'baz'"
                .Given(() =>
                {
                    using (var writer = new StreamWriter(new LocalConfigurator().Path))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Configurator.Add(""foo"", new LocalConfigurationFeature.Foo { Bar = ""baz"" });");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(new LocalConfigurator().Path));

            "When I get the Foo"
                .When(() => result = Configurator.Get<Foo>("foo"))
                .Teardown(() => Configurator.Unload());

            "Then the Foo has a Bar of 'baz'"
                .Then(() => result.Bar.Should().Be("baz"));
        }

        [Scenario]
        public static void RetreivingAnObjectByType(Foo result)
        {
            "Given a local config file containing a Foo with a Bar of 'baz'"
                .Given(() =>
                {
                    using (var writer = new StreamWriter(new LocalConfigurator().Path))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Configurator.Add(""foo"", new LocalConfigurationFeature.Foo { Bar = ""baz"" });");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(new LocalConfigurator().Path));

            "When I get the Foo"
                .When(() => result = Configurator.Get<Foo>())
                .Teardown(() => Configurator.Unload());

            "Then the Foo has a Bar of 'baz'"
                .Then(() => result.Bar.Should().Be("baz"));
        }

        [Scenario]
        public static void RetreivingAnObjectByTypeFromFileWithMultipleTypes(int result)
        {
            "Given a local config file containing items with multiple types"
                .Given(() =>
                {
                    using (var writer = new StreamWriter(new LocalConfigurator().Path))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Configurator.Add(""foo"", new LocalConfigurationFeature.Foo { Bar = ""baz"" });");
                        writer.WriteLine(@"Configurator.Add(""stringId"", ""34"");");
                        writer.WriteLine(@"Configurator.Add(""id"", 12);");
                        writer.WriteLine(@"Configurator.Add(""foo 2"", new LocalConfigurationFeature.Foo { Bar = ""baz 2"" });");
                        writer.WriteLine(@"Configurator.Add(""code"", 15);");                        
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(new LocalConfigurator().Path));

            "When I get int item"
                .When(() => result = Configurator.Get<int>())
                .Teardown(() => Configurator.Unload());

            "Then it should be '12'"
                .Then(() => result.Should().Be(12));
        }

        [Scenario]
        public static void TryingToRetreiveANonExistentTypeObject(Exception ex)
        {
            "Given a local config file not containing any string item"
                .Given(() =>
                {
                    using (var writer = new StreamWriter(new LocalConfigurator().Path))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Configurator.Add(""foo"", new LocalConfigurationFeature.Foo { Bar = ""baz"" });");                        
                        writer.WriteLine(@"Configurator.Add(""id"", 12);");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(new LocalConfigurator().Path));

            "When I try to get string item"
                .When(() => ex = Record.Exception(() => Configurator.Get<string>()))
                .Teardown(() => Configurator.Unload());

            "Then an exception is thrown"
                .Then(() => ex.Should().NotBeNull());
        }

        public class Foo
        {
            public string Bar { get; set; }
        }
    }
}
