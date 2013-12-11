// <copyright file="ConfigRScriptHostFactory.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Scripting
{
    using System;
    using ScriptCs.Contracts;

    public class ConfigRScriptHostFactory : IConfigRScriptHostFactory
    {
        [CLSCompliant(false)]
        public IConfigRScriptHost CreateScriptHost(ISimpleConfig config, IScriptPackManager scriptPackManager, string[] scriptArgs)
        {
            return new ConfigRScriptHost(config, scriptPackManager, scriptArgs);
        }
    }
}
