using System;
using System.ComponentModel;
using static TinyTasksKit.Worker.Preferences.PreferenceDataType;
using static TinyTasksKit.Worker.Preferences.PreferenceSet;

namespace TinyTasksKit.Worker.Preferences
{
    public class ScalarPreference<T> : IPreference
    {
        private readonly IPreferenceProvider provider;

        public ScalarPreference(IPreferenceProvider preferences, string name, T value)
        {
            provider = preferences ?? throw new ArgumentNullException(nameof(preferences));
            Name = name;
            HasDefaultValue = !IsNullOrTypeDefault(value);
            if (HasDefaultValue && IsNullOrTypeDefault(Value)) Value = value;
        }

        public PreferenceDataType DataType { get; private set; } = Primitive;

        public bool Visible { get; private set; } = true;

        public bool HasDefaultValue { get; }

        public bool HasValueSet => HasDefaultValue || !IsNullOrTypeDefault(Value);

        public bool Complete => Visible && HasValueSet;

        public string Name { get; }

        public T Value
        {
            get => provider[Name] is T result ? result : default;
            set => provider[Name] = value;
        }

        public Type ValueType => typeof(T);

        public string ToView()
        {
            return Value?.ToString();
        }

        public void FromView(string line)
        {
            if (TryParseValue<T>(line, out var result))
            {
                Value = result;
            }
        }

        public ScalarPreference<T> ToggleVisibility()
        {
            Visible = !Visible;
            return this;
        }

        public ScalarPreference<T> UpdateDataType(PreferenceDataType dataType)
        {
            DataType = dataType;
            return this;
        }

        private static bool TryParseValue<TTo>(string input, out TTo result)
        {
            var clazz = typeof(TTo);

            if (clazz == typeof(string))
            {
                result = (TTo) Convert.ChangeType(input, typeof(TTo));
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

            Type[] clazzArgs = {typeof(string), clazz.MakeByRefType()};
            var methodInfo = clazz.GetMethod("TryParse", clazzArgs);
            if (methodInfo == null)
            {
                result = default;
                return false;
            }

            object[] args = {input, null};
            var invoke = (bool) methodInfo.Invoke(null, args);
            if (!invoke)
            {
                result = default;
                return false;
            }

            result = (TTo) args[1];
            return true;
        }

        public static implicit operator T(ScalarPreference<T> property)
        {
            return property.Value;
        }
    }
}