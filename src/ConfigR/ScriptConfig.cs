// <copyright file="ScriptConfig.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using ConfigR.Scripting;

    public abstract class ScriptConfig : BasicConfig
    {
        private readonly Assembly[] references;

        protected ScriptConfig(params Assembly[] references)
        {
            this.references = (references ?? Enumerable.Empty<Assembly>()).ToArray();
        }

        public override ISimpleConfig Load()
        {
            this.Load(this.GetScriptPath());
            return this;
        }

        protected virtual void Load(string scriptPath)
        {
            new ScriptConfigLoader(this.references.ToArray()).LoadFromFile(this, scriptPath);
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not appropriate.")]
        protected abstract string GetScriptPath();
    }
}
