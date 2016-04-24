// <copyright file="ConfigRScriptHost.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Scripting
{
    using System;
    using System.Collections.Generic;

    [CLSCompliant(false)]
    public partial class ConfigRScriptHost : IConfigRScriptHost
    {
        private readonly IDictionary<string, object> dictionary;

        public ConfigRScriptHost(IDictionary<string, object> dictionary)
        {
            Guard.AgainstNullArgument("dictionary", dictionary);

            this.dictionary = dictionary;
        }

        public IConfigRScriptHost This
        {
            get { return this; }
        }

        public IConfig Global
        {
            get { return Config.Global; }
        }

        public void Add(object value)
        {
            this.dictionary.Add(value);
        }

        public T Get<T>()
        {
            return this.dictionary.Get<T>();
        }

        public IConfigRScriptHost LoadWebScript(Uri uri)
        {
            Config.Global.LoadWebScript(uri);
            return this;
        }

        public IConfigRScriptHost LoadScriptFile(string path)
        {
            Config.Global.LoadScriptFile(path);
            return this;
        }

        public IConfigRScriptHost LoadLocalScriptFile()
        {
            Config.Global.LoadLocalScriptFile();
            return this;
        }

        public IConfigRScriptHost Load(ISimpleConfig config)
        {
            Config.Global.Load(config);
            return this;
        }
    }
}
