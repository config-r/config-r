// <copyright file="TypedConfigurationItemFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Features
{
    using System;
    using System.IO;
    using FluentAssertions;
    using Xbehave;
    using Xunit;

    public static class TypedConfigurationItemFeature
    {
        [Background]
        public static void Background()
        {
            "Given no configuration has been loaded"
                .f(() => Config.Global.Reset());
        }

        [Scenario]
        public static void GettingATypedConfigurationItem(int result)
        {
            "Given a config file with an integer of 123 named 'foo'"
                .f(() =>
                {
                    using (var writer = new StreamWriter("foo1.csx"))
                    {
                        writer.WriteLine(@"Add(""foo"", 123);");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete("foo1.csx"));

            "When I load the file"
                .f(() => Config.Global.LoadScriptFile("foo1.csx"));

            "And I try to get an integer named 'foo'"
                .f(() => result = Config.Global.Get<int>("foo"));

            "Then the result is 123"
                .f(() => result.Should().Be(123));
        }

        [Scenario]
        public static void TryingToGetAnIncorrectlyTypedConfigurationItem(Exception ex)
        {
            "Given a config file with a string named 'foo'"
                .f(() =>
                {
                    // TODO (Adam): add DSL - new TempFile(string path, string content).Using();
                    using (var writer = new StreamWriter("foo1.csx"))
                    {
                        writer.WriteLine(@"Add(""foo"", ""abc"");");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete("foo1.csx"));

            "When I load the file"
                .f(() => Config.Global.LoadScriptFile("foo1.csx"));

            "And I try to get an integer named 'foo'"
                .f(() => ex = Record.Exception(() => Config.Global.Get<int>("foo")));

            "Then an exception is thrown"
                .f(() => ex.Should().NotBeNull());

            "And the exception message contains 'foo'"
                .f(() => ex.Message.Should().Contain("foo"));

            "And the exception message contains the full type name of int"
                .f(() => ex.Message.Should().Contain(typeof(int).FullName));

            "And the exception message contains the full type name of string"
                .f(() => ex.Message.Should().Contain(typeof(string).FullName));
        }
    }
}