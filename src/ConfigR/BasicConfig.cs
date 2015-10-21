// <copyright file="BasicConfig.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System.Collections.Generic;
    using ConfigR.Logging;

    public partial class BasicConfig : ISimpleConfig
    {
        private static readonly ILog log = LogProvider.For<BasicConfig>();

        private readonly IDictionary<string, object> dictionary = new Dictionary<string, object>();

        protected virtual string Source
        {
            get { return null; }
        }

        public virtual ISimpleConfig Load()
        {
            return this;
        }

        private void LogMutating(string action, string key, object value)
        {
            log.TraceFormat(
                "{0} '{1}' from {2}: {3}", action, key, this.GetSource(), value.ToString());
        }

        private string GetSource()
        {
            return this.Source == null ? "an unknown source" : string.Concat("'", this.Source, "'");
        }
    }
}
