// <copyright file="Config.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System.Collections.Generic;
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

        // dynamic
        public async Task<dynamic> Load() => await this.Load(new DynamicDictionary());

        public async Task<dynamic> Load(object seed) => await this.Load(new DynamicDictionary(seed));

        public async Task<dynamic> Load(IDictionary<string, object> seed) => await this.Load(new DynamicDictionary(seed));

        // dictionary
        public async Task<IDictionary<string, object>> LoadDictionary() => await this.Load();

        public async Task<IDictionary<string, object>> LoadDictionary(object seed) => await this.Load(seed);

        public async Task<IDictionary<string, object>> LoadDictionary(IDictionary<string, object> seed) => await this.Load(seed);

        // private
        private async Task<DynamicDictionary> Load(DynamicDictionary config)
        {
            foreach (var loader in this.loaders)
            {
                config = await loader?.Load(config) ?? config;
            }

            return config;
        }
    }
}
