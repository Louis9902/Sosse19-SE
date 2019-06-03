using System;
using System.ComponentModel;

namespace TinyTasksKit.Worker.Preferences
{
    public class Preference<T> : IPreference
    {
        private readonly IPreferenceProvider provider;

        public Preference(IPreferenceProvider preferences, string name, T value)
        {
            provider = preferences ?? throw new ArgumentNullException(nameof(preferences));
            Name = name;
            HasDefaultValue = !IsDefaultOrNull(value);
            if (HasDefaultValue && IsDefaultOrNull(Value)) Value = value;
        }

        public bool HasDefaultValue { get; }
        public bool IsHidden { get; private set; }

        public string Name { get; }

        public T Value
        {
            get => provider[Name] is T ? (T) provider[Name] : default;
            set => provider[Name] = value;
        }

        public bool IsSatisfied => HasDefaultValue || !IsDefaultOrNull(Value);

        private static bool IsDefaultOrNull<TV>(TV value)
        {
            return Equals(default(TV), value);
        }

        private static bool TryParseValue<T>(string input, out T result)
        {
            var clazz = typeof(T);

            if (clazz == typeof(string))
            {
                result = (T) Convert.ChangeType(input, typeof(T));
                return true;
            }

            var nullable = clazz.IsGenericType && clazz.GetGenericTypeDefinition() == typeof(Nullable<>);
            if (nullable)
            {
                if (string.IsNullOrEmpty(input))
                {
                    result = default;
                    return true;
                }

                clazz = new NullableConverter(clazz).UnderlyingType;
            }

            Type[] argsClazz = {typeof(string), clazz.MakeByRefType()};
            var info = clazz.GetMethod("TryParse", argsClazz);
            if (info == null)
            {
                result = default;
                return false;
            }

            object[] args = {input, null};
            var invoke = (bool) info.Invoke(null, args);
            if (!invoke)
            {
                result = default;
                return false;
            }

            result = (T) args[1];
            return true;
        }

        public Preference<T> ToggleHidden()
        {
            IsHidden = !IsHidden;
            return this;
        }

        public string ToDisplayString()
        {
            return Value is IPreferencePrinter printer ? printer.ToPreferenceString() : Value?.ToString();
        }

        public void FromDisplayString(string line)
        {
            if (TryParseValue<T>(line, out var result))
            {
                Value = result;
            }
        }

        public static implicit operator T(Preference<T> property)
        {
            return property.Value;
        }
    }
}