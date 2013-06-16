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

        void Add(string key, dynamic value);

        void Load();

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get", Justification = "By design.")]
        dynamic Get(string key);

        dynamic GetOrDefault(string key);

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get", Justification = "By design.")]
        T Get<T>(string key);

        T GetOrDefault<T>(string key);
    }
}
