// <copyright file="LoadFromConfiguratorFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Features
{
    using FluentAssertions;
    using Xbehave;

    public static class LoadFromConfiguratorFeature
    {
        [Background]
        public static void Background()
        {
            "Given no configuration has been loaded"
                .Given(() => Config.Global.Reset());
        }

        [Scenario]
        public static void RetreivingAnObject(ISimpleConfig configurator, string result)
        {
            "Given a configurator containing a string of 'bar' keyed by 'foo'"
                .Given(() => (configurator = new BasicConfig()).Add("foo", "bar"));

            "When I load the configurator"
                .When(() => Config.Global.Load(configurator));

            "And I get the Foo"
                .And(() => result = Config.Global.Get<string>("foo"));

            "Then the result should be 'bar'"
                .Then(() => result.Should().Be("bar"));
        }
    }
}
