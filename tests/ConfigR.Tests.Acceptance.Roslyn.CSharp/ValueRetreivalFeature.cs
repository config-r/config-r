// <copyright file="ValueRetreivalFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Tests.Acceptance
{
    using System;
    using ConfigR.Tests.Acceptance.Roslyn.CSharp.Support;
    using FluentAssertions;
    using Xbehave;
    using Xunit;

    public static class ValueRetreivalFeature
    {
        [Scenario]
        public static void RetreivingANonExistentValue(Exception exception)
        {
            dynamic config = null;

            "Given a local config file containing Foo of 42"
                .x(c => ConfigFile.Create("Config.Foo = 42;").Using(c));

            "When I load the config"
                .x(async () => config = await new Config().UseRoslynCSharpLoader().LoadDynamic());

            "And I get Bar"
                .x(() => exception = Record.Exception(() => config.Bar<int>()));

            "Then an exception is thrown"
                .x(() => exception.Should().NotBeNull());

            "And the exception indicates that Bar does not exist"
                .x(() => exception.Message.Should().Contain("'Bar' does not exist"));
        }
    }
}
