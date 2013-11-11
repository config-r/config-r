// <copyright file="LocalConfigurationFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Features
{
    using System.IO;
    using FluentAssertions;
    using Xbehave;

    public static class LocalConfigurationFeature
    {
        [Background]
        public static void Background()
        {
            "Given no configuration is loaded"
                .Given(() => Configurator.Unload());
        }

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
                        writer.WriteLine(@"Add(""foo"", new LocalConfigurationFeature.Foo { Bar = ""baz"" });");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(new LocalConfigurator().Path));

            "When I get the Foo"
                .When(() => result = Configurator.Get<Foo>("foo"));

            "Then the Foo has a Bar of 'baz'"
                .Then(() => result.Bar.Should().Be("baz"));
        }

        [Scenario]
        public static void PassingAValueFromAnAppToAConfigurationScript(string result)
        {
            "Given a local config file which sets foo using the value of bar"
                .Given(() =>
                {
                    using (var writer = new StreamWriter(new LocalConfigurator().Path))
                    {
                        writer.WriteLine(@"Add(""foo"", Configurator.Get<string>(""bar""));");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(new LocalConfigurator().Path));

            "When I set bar to 'baz'"
                .When(() => Configurator.Add("bar", "baz"));

            "And I get foo"
                .And(() => result = Configurator.Get<string>("foo"));

            "Then foo is 'baz'"
                .Then(() => result.Should().Be("baz"));
        }

        public class Foo
        {
            public string Bar { get; set; }
        }
    }
}
