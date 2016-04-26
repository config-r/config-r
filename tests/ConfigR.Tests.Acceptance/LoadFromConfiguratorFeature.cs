// <copyright file="LoadFromConfiguratorFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Tests.Acceptance
{
    using FluentAssertions;
    using Xbehave;

    public static class LoadFromConfiguratorFeature
    {
        [Background]
        public static void Background()
        {
            "Given no configuration has been loaded"
                .f(() => Config.Global.Reset());
        }

        [Scenario]
        public static void RetrievingAnObject(ISimpleConfig configurator, string result)
        {
            "Given a configurator containing a string of 'bar' keyed by 'foo'"
                .f(() => (configurator = new BasicConfig()).Add("foo", "bar"));

            "When I load the configurator"
                .f(() => Config.Global.Load(configurator));

            "And I get the Foo"
                .f(() => result = Config.Global.Get<string>("foo"));

            "Then the result should be 'bar'"
                .f(() => result.Should().Be("bar"));
        }
    }
}
