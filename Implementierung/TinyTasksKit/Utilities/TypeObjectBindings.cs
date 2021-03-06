using System;
using System.Collections.Generic;

namespace TinyTasksKit.Utilities
{
    /// <summary>
    /// Binds C# Types to a specified value, this is used to have a index of classes to a fixed and persistent.  
    /// </summary>
    /// <typeparam name="TBin">The type to witch the value will be bound</typeparam>
    public class TypeObjectBindings<TBin>
    {
        private readonly Dictionary<TBin, Type> mapBinToObj = new Dictionary<TBin, Type>();
        private readonly Dictionary<Type, TBin> mapObjToBin = new Dictionary<Type, TBin>();

        public bool HasBinBinding(TBin bin)
        {
            return mapBinToObj.ContainsKey(bin);
        }

        public bool HasObjBinding(Type type)
        {
            return mapObjToBin.ContainsKey(type);
        }

        /// <summary>
        /// Will register a specified type to a value. This will depending on the mode change the registry.
        /// </summary>
        /// <param name="bin"></param>
        /// <param name="type"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public bool Register(TBin bin, Type type, InsertMode mode = InsertMode.Insert)
        {
            switch (mode)
            {
                case InsertMode.Insert:
                    goto Insert;

                case InsertMode.DupleReturn:
                    if (HasBinBinding(bin) || HasObjBinding(type))
                        return false;
                    goto Insert;

                case InsertMode.DupleThrows:
                    if (HasBinBinding(bin) || HasObjBinding(type))
                        throw new ArgumentException($"Trying to add duplicate binding {bin} for {type}");
                    goto Insert;

                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }

            Insert:
            mapBinToObj[bin] = type;
            mapObjToBin[type] = bin;
            return true;
        }

        public void Register(IEnumerable<KeyValuePair<TBin, Type>> pairs)
        {
            foreach (var pair in pairs)
            {
                Register(pair.Key, pair.Value, InsertMode.DupleReturn);
            }
        }

        public bool GetOrNothing(TBin bin, out Type type)
        {
            return mapBinToObj.TryGetValue(bin, out type);
        }

        public Type GetOrCompute(TBin bin, Func<TBin, Type> func)
        {
            if (mapBinToObj.TryGetValue(bin, out var value)) return value;
            value = func(bin);
            Register(bin, value, InsertMode.DupleThrows);
            return value;
        }

        public bool GetOrNothing(Type type, out TBin bin)
        {
            return mapObjToBin.TryGetValue(type, out bin);
        }

        public TBin GetOrCompute(Type type, Func<Type, TBin> func)
        {
            if (mapObjToBin.TryGetValue(type, out var value)) return value;
            value = func(type);
            Register(value, type, InsertMode.DupleThrows);
            return value;
        }
    }

    public enum InsertMode : byte
    {
        Insert,
        DupleReturn,
        DupleThrows,
    }
}