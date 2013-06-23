// <copyright file="CascadingConfigurator.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CascadingConfigurator : IConfigurator
    {
        private static readonly Comparer<dynamic> comparer = new Comparer<dynamic>();
        private readonly List<IConfigurator> configurators = new List<IConfigurator>();
        private Stack<IConfigurator> currentAdditionTargets = new Stack<IConfigurator>();
        private IConfigurator[] configuratorsAwaitingLoad;

        public IDictionary<string, dynamic> Configuration
        {
            get
            {
                this.EnsureLoaded();

                IEnumerable<KeyValuePair<string, dynamic>> seed = new KeyValuePair<string, dynamic>[0];
                return this.configurators
                    .Aggregate(seed, (current, configurator) => current.Union(configurator.Configuration, comparer))
                    .ToDictionary(pair => pair.Key, pair => pair.Value);
            }
        }

        private bool HasLocal
        {
            get { return this.configurators.Any(configurator => configurator is LocalConfigurator); }
        }

        public dynamic this[string key]
        {
            get
            {
                this.EnsureLoaded();

                foreach (var configurator in this.configurators)
                {
                    dynamic value;
                    if (configurator.TryGet(key, out value))
                    {
                        return value;
                    }
                }

                throw new KeyNotFoundException();
            }
        }

        public IConfigurator Load()
        {
            if (this.EnsureLoaded())
            {
                return this;
            }

            this.configuratorsAwaitingLoad = this.configuratorsAwaitingLoad ?? this.configurators.ToArray();
            this.configurators.Clear();
            foreach (var configurator in this.configuratorsAwaitingLoad)
            {
                this.Load(configurator);
            }

            this.configuratorsAwaitingLoad = null;
            return this;
        }

        public CascadingConfigurator Load(string path)
        {
            return this.Load(new FileConfigurator(path));
        }

        public CascadingConfigurator LoadLocal()
        {
            if (!this.HasLocal)
            {
                this.Load(new LocalConfigurator());
            }

            return this;
        }

        public CascadingConfigurator Load(IConfigurator configurator)
        {
            Guard.AgainstNullArgument("configurator", configurator);

            this.currentAdditionTargets.Push(configurator);
            try
            {
                this.configurators.Add(configurator);
                try
                {
                    configurator.Load();
                }
                catch
                {
                    this.configurators.Remove(configurator);
                    throw;
                }
            }
            finally
            {
                if (this.currentAdditionTargets.Count > 1)
                {
                    this.currentAdditionTargets.Pop();
                }
            }

            return this;
        }

        public IConfigurator Add(string key, dynamic value)
        {
            this.EnsureLoaded();

            this.currentAdditionTargets.Peek().Add(key, value);
            return this;
        }

        public CascadingConfigurator Unload()
        {
            this.configurators.Clear();
            return this;
        }

        private bool EnsureLoaded()
        {
            if (this.configurators.Count == 0)
            {
                this.LoadLocal();
                return true;
            }

            return false;
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
