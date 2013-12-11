// <copyright file="ConfigRScriptHost.Dictionary.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Scripting
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;

    public partial class ConfigRScriptHost
    {
        public ICollection<string> Keys
        {
            get { return this.dictionary.Keys; }
        }

        public ICollection<object> Values
        {
            get { return this.dictionary.Values; }
        }

        public int Count
        {
            get { return this.dictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return this.dictionary.IsReadOnly; }
        }

        public object this[string key]
        {
            get { return this.dictionary.FriendlyGet(key); }
            set { this.dictionary[key] = value; }
        }

        public void Add(string key, object value)
        {
            this.dictionary.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return this.dictionary.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return this.dictionary.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return this.dictionary.TryGetValue(key, out value);
        }

        public void Clear()
        {
            this.dictionary.Clear();
        }

        public void Add(KeyValuePair<string, object> item)
        {
            this.dictionary.Add(item);
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return this.dictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            this.dictionary.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return this.dictionary.Remove(item);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }
    }
}
