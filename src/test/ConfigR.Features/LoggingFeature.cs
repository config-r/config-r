// <copyright file="LoggingFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Features
{
    using System;
    using System.IO;
    using FluentAssertions;
    using Xbehave;
    using Xunit;

    public static class LoggingFeature
    {
        [Scenario]
        public static void ConfigValueHasActionProperty(Exception exception)
        {
            "Given a local config file containing a value with an action property"
                .Given(() =>
                {
                    using (var writer = new StreamWriter(new LocalConfigurator().Path))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Add(""foo"", new LoggingFeature.Foo { Action = () => Console.WriteLine(""hello world"") });");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(new LocalConfigurator().Path));

            "When I get the value"
                .When(() => exception = Record.Exception(() => Configurator.Get<Foo>("foo")))
                .Teardown(() => Configurator.Unload());

            "Then no exception is thrown"
                .Then(() => exception.Should().BeNull());
        }

        public class Foo
        {
            public Action Action { get; set; }
        }
    }
}
