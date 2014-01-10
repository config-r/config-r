// <copyright file="ConfigRScriptHostFactory.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Scripting
{
    using System;
    using ScriptCs;
    using ScriptCs.Contracts;

    public class ConfigRScriptHostFactory : IScriptHostFactory
    {
        private readonly ISimpleConfig config;

        public ConfigRScriptHostFactory(ISimpleConfig config)
        {
            this.config = config;
        }

        [CLSCompliant(false)]
        public IScriptHost CreateScriptHost(IScriptPackManager scriptPackManager, string[] scriptArgs)
        {
            return new ConfigRScriptHost(this.config, scriptPackManager, scriptArgs);
        }
    }
}
