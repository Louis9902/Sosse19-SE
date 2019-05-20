using System;
using System.Collections.Generic;

namespace Backupper.Common
{
    public class TypeMapping<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> mapKeyToValue;
        private readonly Dictionary<TValue, TKey> mapValueToKey;

        public bool Insert(TKey key, TValue value)
        {
            return TryInsert(key, value, InsertMode.Nothing);
        }

        private bool TryInsert(TKey key, TValue value, InsertMode mode)
        {
            return false;
        }

        public bool TryGet(TKey key, out TValue value)
        {
            return mapKeyToValue.TryGetValue(key, out value);
        }

        public TValue Compute(TKey key, Func<TKey, TValue> func)
        {
            if (mapKeyToValue.TryGetValue(key, out var value)) return value;
            value = func(key);
            TryInsert(key, value, InsertMode.Exception);
            return value;
        }

        public bool TryGet(TValue key, out TKey value)
        {
            return mapValueToKey.TryGetValue(key, out value);
        }

        public TKey Compute(TValue key, Func<TValue, TKey> func)
        {
            if (mapValueToKey.TryGetValue(key, out var value)) return value;
            value = func(key);
            TryInsert(value, key, InsertMode.Exception);
            return value;
        }
    }

    internal enum InsertMode : byte
    {
        Nothing,
        Overwrite,
        Exception,
    }
}