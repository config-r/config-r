// <copyright file="ICascadingConfigurator.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    public interface ICascadingConfigurator : IReadableValues
    {
        ICascadingConfigurator Load(IConfigurator configurator);
    }
}
