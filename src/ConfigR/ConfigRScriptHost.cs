// <copyright file="ConfigRScriptHost.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.Collections.Generic;
    using ScriptCs;
    using ScriptCs.Contracts;

    [CLSCompliant(false)]
    public class ConfigRScriptHost : ScriptHost, IConfigRScriptHost
    {
        private readonly IConfigurator configurator;

        public ConfigRScriptHost(IConfigurator configurator, IScriptPackManager scriptPackManager, string[] scriptArgs)
            : base(scriptPackManager, scriptArgs)
        {
            if (configurator == null)
            {
                throw new ArgumentNullException("configurator");
            }

            this.configurator = configurator;
        }

        public IEnumerable<KeyValuePair<string, dynamic>> Items
        {
            get { return this.configurator.Items; }
        }

        public dynamic this[string key]
        {
            get { return this.configurator[key]; }
        }

        public bool TryGet(string key, out dynamic value)
        {
            return this.configurator.TryGet(key, out value);
        }

        public IConfigRScriptHost Add(string key, dynamic value)
        {
            this.configurator.Add(key, value);
            return this;
        }
    }
}
