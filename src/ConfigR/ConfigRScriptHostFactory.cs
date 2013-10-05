// <copyright file="ConfigRScriptHostFactory.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using ScriptCs.Contracts;

    public class ConfigRScriptHostFactory : IConfigRScriptHostFactory
    {
        [CLSCompliant(false)]
        public IConfigRScriptHost CreateScriptHost(IConfigurator configurator, IScriptPackManager scriptPackManager, string[] scriptArgs)
        {
            return new ConfigRScriptHost(configurator, scriptPackManager, scriptArgs);
        }
    }
}
