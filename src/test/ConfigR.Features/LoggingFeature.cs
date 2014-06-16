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
        [Background]
        public static void Background()
        {
            "Given no configuration has been loaded"
                .Given(() => Config.Global.Reset());
        }

        [Scenario]
        public static void ConfigValueHasActionProperty(Exception exception)
        {
            "Given a local config file containing a value with an action property"
                .Given(() =>
                {
                    using (var writer = new StreamWriter(LocalScriptFileConfig.Path))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Add(""foo"", new Foo { Action = () => Console.WriteLine(""hello world"") });");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(LocalScriptFileConfig.Path));

            "When I get the value"
                .When(() => exception = Record.Exception(() => Config.Global.Get<Foo>("foo")));

            "Then no exception is thrown"
                .Then(() => exception.Should().BeNull());
        }
    }
}
