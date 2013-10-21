// <copyright file="CascadingConfigurator.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class CascadingConfigurator : ICascadingConfigurator
    {
        private static readonly Comparer<dynamic> comparer = new Comparer<dynamic>();
        private readonly List<IConfigurator> configurators = new List<IConfigurator>();

        public bool AnyConfigurators
        {
            get { return this.configurators.Any(); }
        }

        public IEnumerable<KeyValuePair<string, dynamic>> Items
        {
            get
            {
                return this.configurators.Aggregate(
                    (IEnumerable<KeyValuePair<string, dynamic>>)new KeyValuePair<string, dynamic>[0],
                    (current, configurator) => current.Union(configurator.Items, comparer));
            }
        }

        public dynamic this[string key]
        {
            get
            {
                foreach (var configurator in this.configurators)
                {
                    dynamic value;
                    if (configurator.TryGet(key, out value))
                    {
                        return value;
                    }
                }

                throw new KeyNotFoundException(string.Format(CultureInfo.InvariantCulture, "'{0}' not found.", key));
            }
        }

        public ICascadingConfigurator Load(IConfigurator configurator)
        {
            Guard.AgainstNullArgument("configurator", configurator);

            this.configurators.Add(configurator);
            try
            {
                
                this.configurators.Add(configurator);
                try
                {
                    configurator.Load();
                }
                catch (Exception ex)
                {
                    this.configurators.Remove(configurator);
                    if ((int)ex.Data["Error_Code"] != 111)
                        throw;
                }
            }
            catch
            {
                this.configurators.Remove(configurator);
                throw;
            }

            return this;
        }

        public bool TryGet(string key, out dynamic value)
        {
            return this.Items.ToDictionary(pair => pair.Key, pair => pair.Value).TryGetValue(key, out value);
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
