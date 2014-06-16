// <copyright file="Config.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public partial class Config : IConfig
    {
        private static readonly Comparer<object> comparer = new Comparer<object>();

        private readonly List<IDictionary<string, object>> dictionaries = new List<IDictionary<string, object>>();

        private IDictionary<string, object> additionTarget;
        private bool loadInvoked;

        public IConfig Load(ISimpleConfig config)
        {
            this.loadInvoked = true;

            Guard.AgainstNullArgument("config", config);

            var index = this.dictionaries.Count;
            this.dictionaries.Add(config);
            try
            {
                config.Load();
            }
            catch
            {
                this.dictionaries.RemoveRange(index, this.dictionaries.Count - index);
                throw;
            }

            return this;
        }

        public IConfig Unload()
        {
            this.dictionaries.Clear();
            return this;
        }

        public IConfig Reset()
        {
            this.Unload();
            this.loadInvoked = false;
            return this;
        }

        public IConfig EnsureLoaded(params Assembly[] references)
        {
            if (this.loadInvoked)
            {
                return this;
            }

            this.loadInvoked = true;
            return this.Load(new LocalScriptFileConfig(true, references));
        }

        private IDictionary<string, object> Cascade()
        {
            return this.dictionaries.Aggregate(
                    (IEnumerable<KeyValuePair<string, object>>)new KeyValuePair<string, object>[0],
                    (current, dictionary) => current.Union<KeyValuePair<string, object>>(dictionary, comparer))
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        private IDictionary<string, object> InternAdditionTarget()
        {
            if (!this.dictionaries.Any() || this.dictionaries.Last() != this.additionTarget)
            {
                this.dictionaries.Add(this.additionTarget = new BasicConfig());
            }

            return this.additionTarget;
        }

        private class Comparer<TValue> : IEqualityComparer<KeyValuePair<string, TValue>>
        {
            public bool Equals(KeyValuePair<string, TValue> x, KeyValuePair<string, TValue> y)
            {
                return string.Equals(x.Key, y.Key, StringComparison.Ordinal);
            }

            public int GetHashCode(KeyValuePair<string, TValue> obj)
            {
                return obj.Key == null ? 0 : obj.Key.GetHashCode();
            }
        }
    }
}
