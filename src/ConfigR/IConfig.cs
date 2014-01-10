// <copyright file="IConfig.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System.Collections.Generic;

    public interface IConfig : IDictionary<string, object>
    {
        IConfig Load(ISimpleConfig config);

        IConfig Unload();

        IConfig Reset();

        IConfig EnsureLoaded();
    }
}
