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
                .And(() => result = Configurator.Get<Foo>("foo"))
                .Teardown(() => Configurator.Unload());

            "Then the Foo has a Bar of 'baz'"
                .Then(() => result.Bar.Should().Be("baz"));
        }

        public class Foo
        {
            public string Bar { get; set; }
        }
    }
}
