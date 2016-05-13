// <copyright file="LocalConfigurationFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Tests.Acceptance.Roslyn.CSharp
{
    using System;
    using ConfigR.Tests.Acceptance.Roslyn.CSharp.Support;
    using FluentAssertions;
    using Xbehave;
    using Xunit;

    public static class LocalConfigurationFeature
    {
        [Scenario]
        public static void RetrievingAnObject(Foo result)
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

        [Scenario]
        public static void ScriptFailsToCompile(Exception exception)
        {
            "Given a local config file which fails to compile"
                .f(c => ConfigFile.Create(@"This is not C#!").Using(c));

            "When I load the config file"
                .f(async () => exception = await Record.ExceptionAsync(async () => await new Config().UseRoslynCSharpLoader().Load()));

            "Then an exception is thrown"
                .f(() => exception.Should().NotBeNull());

            "And the exception should be a compilation error exception"
                .f(() => exception.GetType().Name.Should().Be("CompilationErrorException"));
        }

        [Scenario]
        public static void ScriptFailsToExecute(Exception exception)
        {
            "Given a local config file which throws an exception with the message 'Boo!'"
                .f(c => ConfigFile.Create(@"throw new System.Exception(""Boo!"");").Using(c));

            "When I load the config file"
                .f(async () => exception = await Record.ExceptionAsync(async () => await new Config().UseRoslynCSharpLoader().Load()));

            "Then an exception is thrown"
                .f(() => exception.Should().NotBeNull());

            "And the exception message is 'Boo!'"
                .f(() => exception.Message.Should().Be("Boo!"));
        }
    }
}
