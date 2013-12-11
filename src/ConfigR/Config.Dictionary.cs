// <copyright file="Config.Dictionary.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class Config : IConfig
    {
        public ICollection<string> Keys
        {
            get { return this.Cascade().Keys; }
        }

        public ICollection<object> Values
        {
            get { return this.Cascade().Values; }
        }

        public int Count
        {
            get { return this.Cascade().Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public object this[string key]
        {
            get { return this.Cascade().FriendlyGet(key); }
            set { this.InternAdditionTarget()[key] = value; }
        }

        public void Add(string key, object value)
        {
            this.InternAdditionTarget().Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return this.Cascade().ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return this.dictionaries.Aggregate(false, (current, dictionary) => current || dictionary.Remove(key));
        }

        public bool TryGetValue(string key, out object value)
        {
            return this.Cascade().TryGetValue(key, out value);
        }

        public void Clear()
        {
            this.dictionaries.ForEach(dictionary => dictionary.Clear());
        }

        public void Add(KeyValuePair<string, object> item)
        {
            this.InternAdditionTarget().Add(item);
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return this.Cascade().Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            this.Cascade().CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return this.dictionaries.Aggregate(false, (current, dictionary) => current || dictionary.Remove(item));
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return this.Cascade().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Cascade().GetEnumerator();
        }
    }
}
