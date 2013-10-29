// <copyright file="IConfigRScriptHostFactory.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Scripting
{
    using System;
    using ScriptCs.Contracts;

    [CLSCompliant(false)]
    public interface IConfigRScriptHostFactory
    {
        IConfigRScriptHost CreateScriptHost(IConfigurator configurator, IScriptPackManager scriptPackManager, string[] scriptArgs);
    }
}
