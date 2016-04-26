// <copyright file="ScriptIterationFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Tests.Acceptance
{
    using System.IO;
    using FluentAssertions;
    using Xbehave;

    public static class ScriptIterationFeature
    {
        [Background]
        public static void Background()
        {
            "Given no configuration has been loaded"
                .f(() => Config.Global.Reset());
        }

        [Scenario]
        public static void AScriptIteratingOverItself(int count)
        {
            "Given a local config file containing 2 values which iterates itself and counts the values"
                .f(() =>
                {
                    using (var writer = new StreamWriter(LocalScriptFileConfig.Path))
                    {
                        writer.WriteLine(
@"Add(""foo1"", 123);
Add(""foo2"", 234);

var count = 0;
foreach (var pair in This)
{
    ++count;
}

Add(""count"", count);
");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(LocalScriptFileConfig.Path));

            "When I get the count"
                .f(() => count = Config.Global.Get<int>("count"));

            "Then the count is 2"
                .f(() => count.Should().Be(2));
        }
    }
}
