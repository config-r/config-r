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
        [Scenario]
        public static void GettingATypedConfigurationItem(int result)
        {
            "Given a config file with an integer of 123 named 'foo'"
                .f(() =>
                {
                    using (var writer = new StreamWriter("foo1.csx"))
                    {
                        writer.WriteLine(@"Configurator.Add(""foo"", 123);");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete("foo1.csx"));

            "When I load the file"
                .When(() => Configurator.Load("foo1.csx"));

            "And I try to get an integer named 'foo'"
                .f(() => result = Configurator.Get<int>("foo"))
                .Teardown(() => Configurator.Unload());

            "Then the result is 123"
                .f(() => result.Should().Be(123));
        }

        [Scenario]
        public static void TryingToGetAnIncorrectlyTypedConfigurationItem(Exception ex)
        {
            "Given a config file with a string named 'foo'"
                .f(() =>
                {
                    using (var writer = new StreamWriter("foo1.csx"))
                    {
                        writer.WriteLine(@"Configurator.Add(""foo"", ""abc"");");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete("foo1.csx"));

            "When I load the file"
                .When(() => Configurator.Load("foo1.csx"));

            "And I try to get an integer named 'foo'"
                .f(() => ex = Record.Exception(() => Configurator.Get<int>("foo")))
                .Teardown(() => Configurator.Unload());

            "Then an exception is thrown"
                .f(() => ex.Should().NotBeNull());

            "And the exception message contains 'foo'"
                .f(() => ex.Message.Should().Contain("foo"));

            "And the exception contains an inner exception"
                .f(() => ex.InnerException.Should().NotBeNull());

            "And the inner exception message contains 'string'"
                .f(() => ex.InnerException.Message.Should().Contain("string"));

            "And the inner exception message contains 'int'"
                .f(() => ex.InnerException.Message.Should().Contain("int"));
        }
    }
}