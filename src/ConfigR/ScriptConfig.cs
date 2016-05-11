// <copyright file="ScriptConfig.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

using Microsoft.CodeAnalysis;

namespace ConfigR
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using ConfigR.Scripting;

    public abstract class ScriptConfig : BasicConfig
    {
        private readonly Assembly[] references;
        private MetadataReference[] metadataReferences;

        protected ScriptConfig(params Assembly[] references)
        {
            this.references = (references ?? Enumerable.Empty<Assembly>()).ToArray();
        }

#pragma warning disable CS3001
        protected ScriptConfig(Assembly[] references, MetadataReference[] metadataReferences)
#pragma warning restore CS3001
        {
            this.references = (references ?? Enumerable.Empty<Assembly>()).ToArray();
            this.metadataReferences = (metadataReferences ?? Enumerable.Empty<MetadataReference>()).ToArray();
        }

        public override ISimpleConfig Load()
        {
            this.Load(this.GetScriptPath());
            return this;
        }

        protected virtual void Load(string scriptPath)
        {
            new ScriptConfigLoader(this.references, this.metadataReferences).LoadFromFile(this, scriptPath);
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not appropriate.")]
        protected abstract string GetScriptPath();
    }
}
