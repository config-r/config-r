// <copyright file="IConfigRScriptHost.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Scripting
{
    using System;
    using ScriptCs;

    [CLSCompliant(false)]
    public interface IConfigRScriptHost : IScriptHost, IConfiguration
    {
        IConfigRScriptHost Add(string key, dynamic value);

        IConfigRScriptHost Add(dynamic value);

        IConfigRScriptHost Load(Uri uri);

        IConfigRScriptHost Load(string path);

        IConfigRScriptHost LoadLocal();

        IConfigRScriptHost Load(IConfigurator nestedConfigurator);
    }
}
