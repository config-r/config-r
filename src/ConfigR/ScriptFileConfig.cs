// <copyright file="ScriptFileConfig.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System.Reflection;

    public class ScriptFileConfig : ScriptConfig
    {
        private readonly string path;

        public ScriptFileConfig(string path, params Assembly[] references)
            : base(references)
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

        protected override string GetScriptPath()
        {
            return this.path;
        }
    }
}
