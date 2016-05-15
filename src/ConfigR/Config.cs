// <copyright file="Config.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using ConfigR.Sdk;

    public class Config : IConfig
    {
        private readonly Queue<ILoader> loaders = new Queue<ILoader>();

        public IConfig UseLoader(ILoader loader)
        {
            this.loaders.Enqueue(loader);
            return this;
        }

        public Task<dynamic> Load() => this.Load(new DynamicDictionary());

        public async Task<dynamic> Load(object seed)
        {
            var config = seed as DynamicDictionary;
            if (config == null)
            {
                config = new DynamicDictionary();
                if (seed != null)
                {
                    foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(seed.GetType()))
                    {
                        config.Add(property.Name, property.GetValue(seed));
                    }
                }
            }

            foreach (var loader in this.loaders)
            {
                config = await loader?.Load(config) ?? config;
            }

            return config;
        }

        public async Task<IDictionary<string, object>> LoadDictionary() => await this.Load();

        public async Task<IDictionary<string, object>> LoadDictionary(object seed) => await this.Load(seed);
    }
}
