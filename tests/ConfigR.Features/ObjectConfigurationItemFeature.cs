// <copyright file="ObjectConfigurationItemFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Features
{
    using System.IO;
    using FluentAssertions;
    using Xbehave;

    public static class ObjectConfigurationItemFeature
    {
        [Background]
        public static void Background()
        {
            "Given no configuration has been loaded"
                .f(() => Config.Global.Reset());
        }

        [Scenario]
        public static void TryingToGetAnObjectConfigurationItem(object result)
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

            "And I try to get an object named 'foo'"
                .f(() => Config.Global.TryGetValue("foo", out result));

            "Then the result is 123"
                .f(() => result.Should().Be(123));
        }
    }
}