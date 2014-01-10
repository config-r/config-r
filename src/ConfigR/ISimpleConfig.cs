// <copyright file="ISimpleConfig.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System.Collections.Generic;

    public interface ISimpleConfig : IDictionary<string, object>
    {
        ISimpleConfig Load();
    }
}
