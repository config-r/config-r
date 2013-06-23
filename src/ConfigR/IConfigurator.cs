// <copyright file="IConfigurator.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public interface IConfigurator
    {
        IDictionary<string, dynamic> Configuration { get; }

        dynamic this[string key] { get; }

        IConfigurator Load();

        IConfigurator Add(string key, dynamic value);
    }
}
