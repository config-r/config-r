// <copyright file="ScriptFileConfig.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using ConfigR.Scripting;

    public class ScriptFileConfig : BasicConfig
    {
        private readonly string path;

        public ScriptFileConfig(string path)
        {
            this.path = path;
        }

        public virtual string Path
        {
            get { return this.path; }
        }

        protected override string Source
        {
            get { return this.Path; }
        }

        public override ISimpleConfig Load()
        {
            new ScriptConfigLoader().LoadFromFile(this, this.Path);
            return this;
        }
    }
}
