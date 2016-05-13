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

        public async Task<dynamic> Load()
        {
            var config = new DynamicDictionary();
            foreach (var loader in this.loaders)
            {
                config = await loader?.Load(config) ?? config;
            }

            return config;
        }
    }
}
