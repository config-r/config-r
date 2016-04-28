// <copyright file="LoggingFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Tests.Acceptance
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
                .f(() => Config.Global.Reset());
        }

        [Scenario]
        public static void ConfigValueHasActionProperty(Exception exception)
        {
            "Given a local config file containing a value with an action property"
                .f(() =>
                {
                    using (var writer = new StreamWriter(LocalScriptFileConfig.Path))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Tests.Acceptance.dll""");
                        writer.WriteLine(@"using ConfigR.Tests.Acceptance;");
                        writer.WriteLine(@"Add(""foo"", new Foo { Action = () => Console.WriteLine(""hello world"") });");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(LocalScriptFileConfig.Path));

            "When I get the value"
                .f(() => exception = Record.Exception(() => Config.Global.Get<Foo>("foo")));

            "Then no exception is thrown"
                .f(() => exception.Should().BeNull());
        }
    }
}
