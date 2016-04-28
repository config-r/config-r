// <copyright file="LocalConfigurationFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Tests.Acceptance.Roslyn.CSharp
{
    using System;
    using System.IO;
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
                .f(() =>
                {
                    path = Path.ChangeExtension(
                        AppDomain.CurrentDomain.SetupInformation.VSHostingAgnosticConfigurationFile(), "csx");

                    using (var writer = new StreamWriter(path))
                    {
                        writer.WriteLine(@"using ConfigR.Tests.Acceptance.Roslyn.CSharp.Support;");
                        writer.WriteLine(@"Config.Foo = new Foo { Bar = ""baz"" };");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(path));

            "And the config is loaded"
                .f(async () => config = await new Config().UseRoslynCSharpLoader().Load());

            "When I get the Foo"
                .f(() => result = config.Foo<Foo>());

            "Then the Foo has a Bar of 'baz'"
                .f(() => result.Bar.Should().Be("baz"));
        }

        ////[Scenario]
        ////public static void PassingAValueFromAnAppToAConfigurationScript(string result)
        ////{
        ////    "Given a local config file which sets foo using the value of bar"
        ////        .f(() =>
        ////        {
        ////            using (var writer = new StreamWriter(LocalScriptFileConfig.Path))
        ////            {
        ////                writer.WriteLine(@"Add(""foo"", Config.Global.Get<string>(""bar""));");
        ////                writer.Flush();
        ////            }
        ////        })
        ////        .Teardown(() => File.Delete(LocalScriptFileConfig.Path));

        ////    "When I set bar to 'baz'"
        ////        .f(() => Config.DisableGlobalAutoLoading().Add("bar", "baz"));

        ////    "And I get foo"
        ////        .f(() => result = Config.EnableGlobalAutoLoading().Get<string>("foo"));

        ////    "Then foo is 'baz'"
        ////        .f(() => result.Should().Be("baz"));
        ////}

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
