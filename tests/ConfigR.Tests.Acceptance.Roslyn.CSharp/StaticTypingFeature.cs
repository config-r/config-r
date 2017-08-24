// <copyright file="StaticTypingFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Tests.Acceptance
{
    using System;
    using ConfigR.Tests.Acceptance.Roslyn.CSharp.Support;
    using FluentAssertions;
    using Xbehave;
    using Xunit;

    public static class StaticTypingFeature
    {
        [Scenario]
        public static void TryingToGetAnIncorrectlyTypedConfigurationItem(Exception ex)
        {
            dynamic config = null;

            "Given a config file with a Foo string"
                .x(c => ConfigFile.Create(@"Config.Foo = ""abc"";").Using(c));

            "When I load the file"
                .x(async () => config = await new Config().UseRoslynCSharpLoader().LoadDynamic());

            "And I try to get an integer named 'foo'"
                .x(() => ex = Record.Exception(() => config.Foo<int>()));

            "Then an exception is thrown"
                .x(() => ex.Should().NotBeNull());

            "And the exception message contains 'Foo'"
                .x(() => ex.Message.Should().Contain("Foo"));

            "And the exception message contains the full type name of int"
                .x(() => ex.Message.Should().Contain(typeof(int).FullName));

            "And the exception message contains the full type name of string"
                .x(() => ex.Message.Should().Contain(typeof(string).FullName));
        }

        [Scenario]
        public static void TryingToGetANullConfigurationItem(Exception ex)
        {
            dynamic config = null;

            "Given a config file with a Foo null"
                .x(c => ConfigFile.Create(@"Config.Foo = null;").Using(c));

            "When I load the file"
                .x(async () => config = await new Config().UseRoslynCSharpLoader().LoadDynamic());

            "And I try to get an integer named 'foo'"
                .x(() => ex = Record.Exception(() => config.Foo<int>()));

            "Then an exception is thrown"
                .x(() => ex.Should().NotBeNull());

            "And the exception message contains 'Foo'"
                .x(() => ex.Message.Should().Contain("Foo"));

            "And the exception message contains the full type name of int"
                .x(() => ex.Message.Should().Contain(typeof(int).FullName));

            "And the exception message contains 'null'"
                .x(() => ex.Message.Should().Contain("null"));
        }
    }
}
