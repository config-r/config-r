// <copyright file="CascadingConfiguratorExtensions.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public static class CascadingConfiguratorExtensions
    {
        public static ICascadingConfigurator Load(this ICascadingConfigurator configurator, Uri uri)
        {
            Guard.AgainstNullArgument("configurator", configurator);

            return configurator.Load(new WebConfigurator(uri));
        }

        [SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads", Justification = "It's not a string URI, it's a path.")]
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
            return configurator.Add(Guid.NewGuid().ToString(), value);
        }
    }
}
