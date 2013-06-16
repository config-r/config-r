// <copyright file="LocalConfiguratorFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Features
{
    using System.IO;
    using System.Reflection;
    using FluentAssertions;
    using Xbehave;

    public static class LocalConfiguratorFeature
    {
        [Scenario]
        public static void Configuration()
        {
            "Given a config file"
                .Given(() =>
                {
                    using (var writer = new StreamWriter(LocalConfigurator.Path))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Configurator.Add(""foo"", new LocalConfiguratorFeature.Foo { Bar = ""baz"" });");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(LocalConfigurator.Path));

            "When I load the file"
                .When(() => Configurator.Load());

            "Then the configuration is added"
                .Then(() => Configurator.Get<Foo>("foo").Bar.Should().Be("baz"));
        }

        public class Foo
        {
            public string Bar { get; set; }
        }
    }
}
