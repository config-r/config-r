// <copyright file="LocalConfigurationFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Tests.Acceptance.Roslyn.CSharp
{
    using ConfigR.Tests.Acceptance.Roslyn.CSharp.Support;
    using FluentAssertions;
    using Xbehave;

    public static class LocalConfigurationFeature
    {
        [Scenario]
        public static void RetrievingAnObject(string path, Foo result)
        {
            dynamic config = null;

            "Given a local config file containing a Foo with a Bar of 'baz'"
                .f(c =>
                {
                    var code =
@"using ConfigR.Tests.Acceptance.Roslyn.CSharp.Support;
Config.Foo = new Foo { Bar = ""baz"" };
";

                    ConfigFile.Create(code).Using(c);
                });

            "When I load the config"
                .f(async () => config = await new Config().UseRoslynCSharpLoader().Load());

            "And I get the Foo"
                .f(() => result = config.Foo<Foo>());

            "Then the Foo has a Bar of 'baz'"
                .f(() => result.Bar.Should().Be("baz"));
        }

        [Scenario]
        public static void PassingAValueFromAnAppToAConfigurationScript(string result)
        {
            dynamic config = null;

            "Given a local config file which sets Foo using the value of Bar"
                .f(c => ConfigFile.Create(@"Config.Foo = Config.Bar;").Using(c));

            "And I load the config seeded with Bar set to 'baz'"
                .f(async () => config = await new Config().UseRoslynCSharpLoader().Load(new { Bar = "baz" }));

            "And I get Foo"
                .f(() => result = config.Foo<string>());

            "Then Foo is 'baz'"
                .f(() => result.Should().Be("baz"));
        }

        ////[Scenario]
        ////public static void ScriptIsMissingAClosingParenthesis(object exception)
        ////{
        ////    "Given a local config file which is missing a closing bracket"
        ////        .f(() =>
        ////        {
        ////            using (var writer = new StreamWriter(LocalScriptFileConfig.Path))
        ////            {
        ////                writer.WriteLine(@"Add(""foo"", 123;");
        ////                writer.Flush();
        ////            }
        ////        })
        ////        .Teardown(() => File.Delete(LocalScriptFileConfig.Path));

        ////    "When I load the config file"
        ////        .f(() => exception = Record.Exception(() => Config.Global));

        ////    "Then an exception is thrown"
        ////        .f(() => exception.Should().NotBeNull());
        ////}

        ////[Scenario]
        ////public static void ScriptFailsToExecute(object exception)
        ////{
        ////    "Given a local config file which fails to execute"
        ////        .f(() =>
        ////        {
        ////            using (var writer = new StreamWriter(LocalScriptFileConfig.Path))
        ////            {
        ////                writer.WriteLine("throw new Exception();");
        ////                writer.Flush();
        ////            }
        ////        })
        ////        .Teardown(() => File.Delete(LocalScriptFileConfig.Path));

        ////    "When I load the config file"
        ////        .f(() => exception = Record.Exception(() => Config.Global));

        ////    "Then an exception is thrown"
        ////        .f(() => exception.Should().NotBeNull());
        ////}
    }
}
