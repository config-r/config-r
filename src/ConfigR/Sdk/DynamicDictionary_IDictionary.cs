// <copyright file="DynamicDictionary_IDictionary.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Sdk
{
    using System.Collections;
    using System.Collections.Generic;

    public partial class DynamicDictionary : IDictionary<string, object>
    {
        public int Count
        {
            get
            {
                return this.values.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return ((IDictionary<string, object>)this.values).IsReadOnly;
            }
        }

        public ICollection<string> Keys
        {
            get
            {
                return ((IDictionary<string, object>)this.values).Keys;
            }
        }

        public ICollection<object> Values
        {
            get
            {
                return ((IDictionary<string, object>)this.values).Values;
            }
        }

        public object this[string key]
        {
            get
            {
                return this.values[key];
            }

            set
            {
                this.values[key] = value;
            }
        }

        public void Add(KeyValuePair<string, object> item)
        {
            ((IDictionary<string, object>)this.values).Add(item);
        }

        public void Add(string key, object value)
        {
            this.values.Add(key, value);
        }

        public void Clear()
        {
            this.values.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return ((IDictionary<string, object>)this.values).Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return this.values.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            ((IDictionary<string, object>)this.values).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return ((IDictionary<string, object>)this.values).GetEnumerator();
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return ((IDictionary<string, object>)this.values).Remove(item);
        }

        public bool Remove(string key)
        {
            return this.values.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return this.values.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<string, object>)this.values).GetEnumerator();
        }
    }
}
