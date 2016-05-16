// <copyright file="RoslynCSharpConfig.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Roslyn.CSharp.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;
    using ConfigR.Sdk;
    using Microsoft.CodeAnalysis;

    public class RoslynCSharpConfig : IRoslynCSharpConfig
    {
        private readonly IConfig config;
        private readonly RoslynCSharpLoader loader;

        public RoslynCSharpConfig(IConfig config, RoslynCSharpLoader loader)
        {
            Guard.AgainstNullArgument(nameof(config), config);
            Guard.AgainstNullArgument(nameof(loader), loader);

            this.config = config;
            this.loader = loader;
        }

        public Task<dynamic> Load()
        {
            return this.config.Load();
        }

        public Task<dynamic> Load(object seed)
        {
            return this.config.Load(seed);
        }

        public Task<IDictionary<string, object>> LoadDictionary()
        {
            return this.config.LoadDictionary();
        }

        public Task<IDictionary<string, object>> LoadDictionary(object seed)
        {
            return this.config.LoadDictionary(seed);
        }

        public IConfig UseLoader(ILoader loader)
        {
            return this.config.UseLoader(loader);
        }

        [CLSCompliant(false)]
        public IRoslynCSharpConfig AddReferences(params MetadataReference[] references)
        {
            this.loader.AddReferences(references);
            return this;
        }

        [CLSCompliant(false)]
        public IRoslynCSharpConfig AddReferences(params Assembly[] references)
        {
            this.loader.AddReferences(references);
            return this;
        }
    }
}
