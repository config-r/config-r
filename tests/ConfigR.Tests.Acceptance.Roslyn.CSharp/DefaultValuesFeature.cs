// <copyright file="DefaultValuesFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Tests.Acceptance
{
    using System.IO;
    using FluentAssertions;
    using Xbehave;

    public static class DefaultValuesFeature
    {
        [Background]
        public static void Background()
        {
            "Given no configuration has been loaded"
                .f(() => Config.Global.Reset());
        }

        [Scenario]
        public static void GettingAnExistingItem(int result)
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

            "And I get an int named 'foo' with a default of 456"
                .f(() => result = Config.Global.GetOrDefault("foo", 456));

            "Then the result is 123"
                .f(() => result.Should().Be(123));
        }

        [Scenario]
        public static void TryingToGetAnExistingItem(bool success, int result)
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

            "And I try to get an int named 'foo' with a default of 456"
                .f(() => success = Config.Global.TryGetValueOrDefault("foo", out result, 456));

            "Then the attempt succeeds"
                .f(() => success.Should().BeTrue());

            "And the result is 123"
                .f(() => result.Should().Be(123));
        }

        [Scenario]
        public static void GettingANonexistentItem(int result)
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

            "And I get an int named 'bar' with a default of 456"
                .f(() => result = Config.Global.GetOrDefault("bar", 456));

            "Then the result is 456"
                .f(() => result.Should().Be(456));
        }

        [Scenario]
        public static void TryingToGetANonexistentItem(bool success, int result)
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

            "And I try to get an int named 'bar' with a default of 456"
                .f(() => success = Config.Global.TryGetValueOrDefault("bar", out result, 456));

            "Then the attempt fails"
                .f(() => success.Should().BeFalse());

            "And the result is 456"
                .f(() => result.Should().Be(456));
        }
    }
}
