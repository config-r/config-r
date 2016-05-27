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

        public async Task<dynamic> LoadDynamic() =>
            await this.Load(new DynamicDictionary());

        public async Task<dynamic> LoadDynamic(object seed) =>
            await this.Load(new DynamicDictionary(seed));

        public async Task<dynamic> LoadDynamic(IDictionary<string, object> seed) =>
            await this.Load(new DynamicDictionary(seed));

        public async Task<IDictionary<string, object>> LoadDictionary() =>
            await this.Load(new DynamicDictionary());

        public async Task<IDictionary<string, object>> LoadDictionary(object seed) =>
            await this.Load(new DynamicDictionary(seed));

        public async Task<IDictionary<string, object>> LoadDictionary(IDictionary<string, object> seed) =>
            await this.Load(new DynamicDictionary(seed));

        public async Task<T> Load<T>() where T : new() =>
            (await this.Load(new DynamicDictionary())).Bind<T>();

        public async Task<T> Load<T>(object seed) where T : new() =>
            (await this.Load(new DynamicDictionary(seed))).Bind<T>();

        public async Task<T> Load<T>(IDictionary<string, object> seed) where T : new() =>
            (await this.Load(new DynamicDictionary(seed))).Bind<T>();

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
