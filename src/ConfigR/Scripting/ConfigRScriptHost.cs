// <copyright file="ConfigRScriptHost.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Scripting
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
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

        public IConfigRScriptHost Add(dynamic value)
        {
            return this.Add(Guid.NewGuid().ToString(), value);
        }

        public IConfigRScriptHost Load(Uri uri)
        {
            Configurator.Load(uri);
            return this;
        }

        [SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads", Justification = "It's not a string URI, it's a path.")]
        public IConfigRScriptHost Load(string path)
        {
            Configurator.Load(path);
            return this;
        }

        public IConfigRScriptHost LoadLocal()
        {
            Configurator.LoadLocal();
            return this;
        }

        public IConfigRScriptHost Load(IConfigurator nestedConfigurator)
        {
            Configurator.Load(nestedConfigurator);
            return this;
        }
    }
}
