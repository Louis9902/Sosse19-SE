using System;
using System.Collections.Generic;

namespace WorksKit.IO
{
    public class TypeBindings<TBin>
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

        public bool Register(TBin bin, Type type, InsertMode mode = InsertMode.Insert)
        {
            switch (mode)
            {
                case InsertMode.Insert:
                    goto Insert;

                case InsertMode.Return:
                    if (HasBinBinding(bin) || HasObjBinding(type))
                        return false;
                    goto Insert;

                case InsertMode.Throws:
                    if (HasBinBinding(bin) || HasObjBinding(type))
                        throw new DuplicateBindingException("Trying to add duplicate binding", bin, type);
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
        }

        public bool GetOrNothing(TBin bin, out Type type)
        {
            return mapBinToObj.TryGetValue(bin, out type);
        }

        public Type GetOrCompute(TBin bin, Func<TBin, Type> func)
        {
            if (mapBinToObj.TryGetValue(bin, out var value)) return value;
            value = func(bin);
            Register(bin, value, InsertMode.Throws);
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
            Register(value, type, InsertMode.Throws);
            return value;
        }
    }

    public enum InsertMode : byte
    {
        Insert,
        Return,
        Throws,
    }

    public sealed class DuplicateBindingException : ArgumentException
    {
        public DuplicateBindingException(string message, object bin, object obj) : base(message)
        {
            Data["bin"] = bin;
            Data["obj"] = obj;
        }
    }
}