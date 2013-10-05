// <copyright file="CascadingConfiguratorExtensions.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;

    public static class CascadingConfiguratorExtensions
    {
        public static ICascadingConfigurator Load(this ICascadingConfigurator configurator, Uri uri)
        {
            Guard.AgainstNullArgument("configurator", configurator);

            return configurator.Load(new WebConfigurator(uri));
        }

        public static ICascadingConfigurator Load(this ICascadingConfigurator configurator, string path)
        {
            Guard.AgainstNullArgument("configurator", configurator);

            return configurator.Load(new FileConfigurator(path));
        }

        public static ICascadingConfigurator LoadLocal(this ICascadingConfigurator configurator)
        {
            Guard.AgainstNullArgument("configurator", configurator);

            return configurator.Load(new LocalConfigurator());
        }

        public static ICascadingConfigurator Add(this ICascadingConfigurator configurator, dynamic value)
        {
            Guard.AgainstNullArgument("configurator", configurator);

            return configurator.Add(Guid.NewGuid().ToString(), value);
        }
    }
}
