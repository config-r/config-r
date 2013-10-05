// <copyright file="ConfiguratorExtensions.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;

    public static class ConfiguratorExtensions
    {
        public static IConfigurator Add(this IConfigurator configurator, dynamic value)
        {
            return configurator.Add(Guid.NewGuid().ToString(), value);
        }
    }
}
