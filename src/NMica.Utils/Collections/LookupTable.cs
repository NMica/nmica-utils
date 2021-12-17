﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace NMica.Utils.Collections
{
    [PublicAPI]
    [Serializable]
    public class LookupTable<TKey, TValue> : ILookup<TKey, TValue>
    {
        private readonly Dictionary<TKey, List<TValue>> _dictionary;

        public LookupTable()
            : this(new Dictionary<TKey, List<TValue>>())
        {
        }

        public LookupTable(IEqualityComparer<TKey> comparer)
            : this(new Dictionary<TKey, List<TValue>>(comparer))
        {
        }

        public LookupTable(ILookup<TKey, TValue> lookupTable, IEqualityComparer<TKey> comparer = null)
            : this(Enumerable.ToDictionary(lookupTable, x => x.Key, x => x.ToList(), comparer))
        {
        }

        public LookupTable(Dictionary<TKey, List<TValue>> dictionary)
        {
            _dictionary = dictionary;
        }

        private ILookup<TKey, TValue> Lookup =>
            _dictionary.SelectMany(x => x.Value.Select(y => new KeyValuePair<TKey, TValue>(x.Key, y)))
                .ToLookup(x => x.Key, x => x.Value);

        public int Count => Lookup.Count;

        public IEnumerable<TValue> this[[NotNull] TKey key] => Lookup[key];

        public void Add(TKey key, TValue value)
        {
            var list = (_dictionary[key] = _dictionary.GetValueOrDefault(key, new List<TValue>())).NotNull();
            list.Add(value);
        }

        public void AddRange(TKey key, IEnumerable<TValue> values)
        {
            var list = (_dictionary[key] = _dictionary.GetValueOrDefault(key, new List<TValue>())).NotNull();
            foreach (var value in values)
                list.Add(value);
        }

        public void Remove(TKey key)
        {
            _dictionary.Remove(key);
        }

        public void Remove(TKey key, TValue value)
        {
            _dictionary.GetValueOrDefault(key)?.Remove(value);
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public IEnumerator<IGrouping<TKey, TValue>> GetEnumerator()
        {
            return Lookup.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Contains([NotNull] TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public ILookup<TKey, TValue> AsReadOnly()
        {
            return this;
        }
    }
}
