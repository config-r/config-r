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
                .f(c => ConfigFile.Create(@"Config.Foo = ""abc"";").Using(c));

            "When I load the file"
                .f(async () => config = await new Config().UseRoslynCSharpLoader().Load());

            "And I try to get an integer named 'foo'"
                .f(() => ex = Record.Exception(() => config.Foo<int>()));

            "Then an exception is thrown"
                .f(() => ex.Should().NotBeNull());

            "And the exception message contains 'Foo'"
                .f(() => ex.Message.Should().Contain("Foo"));

            "And the exception message contains the full type name of int"
                .f(() => ex.Message.Should().Contain(typeof(int).FullName));

            "And the exception message contains the full type name of string"
                .f(() => ex.Message.Should().Contain(typeof(string).FullName));
        }
    }
}
