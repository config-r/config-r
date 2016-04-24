// <copyright file="AnonymousTypesFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Features
{
    using System;
    using System.IO;
    using FluentAssertions;
    using Xbehave;

    public static class AnonymousTypesFeature
    {
        [Background]
        public static void Background()
        {
            "Given no configuration has been loaded"
                .f(() => Config.Global.Reset());
        }

        [Scenario]
        public static void RetrievingAnAnonymousType()
        {
            dynamic result = null;

            "Given a local config file containing an anonymous type with a Bar of 'baz'"
                .f(() =>
                {
                    AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", "Test.config");
                    using (var writer = new StreamWriter(LocalScriptFileConfig.Path))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Add(""Foo"", new { Bar = ""baz"" });");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(LocalScriptFileConfig.Path));

            "When I get the anonymous type"
                .f(() => { result = Config.Global.Get<dynamic>("Foo"); });

            "Then the anonymous type has a Bar of 'baz'"
                .f(() => ((string)result.Bar).Should().Be("baz"));
        }
    }
}
