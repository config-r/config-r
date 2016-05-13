// <copyright file="ScriptIndexingFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Tests.Acceptance
{
    using System.IO;
    using FluentAssertions;
    using Xbehave;

    public static class ScriptIndexingFeature
    {
        [Background]
        public static void Background()
        {
            "Given no configuration has been loaded"
                .f(() => Config.Global.Reset());
        }

        [Scenario]
        public static void AScriptIndexingItself(int value)
        {
            "Given a local config file which indexes itself to set an named 'value' of 123"
                .f(() =>
                {
                    using (var writer = new StreamWriter(LocalScriptFileConfig.Path))
                    {
                        writer.WriteLine(@"This[""value""] = 123;");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(LocalScriptFileConfig.Path));

            "When I get the value"
                .f(() => value = Config.Global.Get<int>("value"));

            "Then the value is 123"
                .f(() => value.Should().Be(123));
        }
    }
}
