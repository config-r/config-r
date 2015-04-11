// <copyright file="LocalConfigurationFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Features
{
    using System.IO;
    using FluentAssertions;
    using Xbehave;
    using Xunit;

    public static class LocalConfigurationFeature
    {
        [Background]
        public static void Background()
        {
            "Given no configuration has been loaded"
                .f(() => Config.Global.Reset());
        }

        [Scenario]
        public static void RetrievingAnObject(Foo result)
        {
            "Given a local config file containing a Foo with a Bar of 'baz'"
                .f(() =>
                {
                    using (var writer = new StreamWriter(LocalScriptFileConfig.Path))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Add(""foo"", new Foo { Bar = ""baz"" });");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(LocalScriptFileConfig.Path));

            "When I get the Foo"
                .f(() => result = Config.Global.Get<Foo>("foo"));

            "Then the Foo has a Bar of 'baz'"
                .f(() => result.Bar.Should().Be("baz"));
        }

        [Scenario]
        public static void PassingAValueFromAnAppToAConfigurationScript(string result)
        {
            "Given a local config file which sets foo using the value of bar"
                .f(() =>
                {
                    using (var writer = new StreamWriter(LocalScriptFileConfig.Path))
                    {
                        writer.WriteLine(@"Add(""foo"", Config.Global.Get<string>(""bar""));");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(LocalScriptFileConfig.Path));

            "When I set bar to 'baz'"
                .f(() => Config.DisableGlobalAutoLoading().Add("bar", "baz"));

            "And I get foo"
                .f(() => result = Config.EnableGlobalAutoLoading().Get<string>("foo"));

            "Then foo is 'baz'"
                .f(() => result.Should().Be("baz"));
        }

        [Scenario]
        public static void ScriptIsMissingAClosingParenthesis(object exception)
        {
            "Given a local config file which is missing a closing bracket"
                .f(() =>
                {
                    using (var writer = new StreamWriter(LocalScriptFileConfig.Path))
                    {
                        writer.WriteLine(@"Add(""foo"", 123;");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(LocalScriptFileConfig.Path));

            "When I load the config file"
                .f(() => exception = Record.Exception(() => Config.Global));

            "Then an exception is thrown"
                .f(() => exception.Should().NotBeNull());
        }
    }
}
