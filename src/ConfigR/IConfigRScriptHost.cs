// <copyright file="IConfigRScriptHost.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using ScriptCs;

    [CLSCompliant(false)]
    public interface IConfigRScriptHost : IScriptHost, IReadableValues
    {
        IConfigRScriptHost Add(string key, dynamic value);
    }
}
