// <copyright file="IConfigRScriptHost.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Scripting
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [CLSCompliant(false)]
    public interface IConfigRScriptHost : IDictionary<string, object>
    {
        IConfigRScriptHost This { get; }

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Global", Justification = "By design.")]
        IConfig Global { get; }

        void Add(object value);

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get", Justification = "By design.")]
        T Get<T>();

        IConfigRScriptHost LoadWebScript(Uri uri);

        IConfigRScriptHost LoadScriptFile(string path);

        IConfigRScriptHost LoadLocalScriptFile();

        IConfigRScriptHost Load(ISimpleConfig config);
    }
}
