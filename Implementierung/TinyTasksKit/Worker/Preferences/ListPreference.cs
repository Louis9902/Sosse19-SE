using System;
using System.Collections.Generic;
using static TinyTasksKit.Worker.Preferences.PreferenceSet;

namespace TinyTasksKit.Worker.Preferences
{
    public class ListPreference<T> : IPreference
    {
        private readonly IPreferenceProvider provider;

        public ListPreference(IPreferenceProvider preferences, string name, IList<T> value)
        {
            provider = preferences ?? throw new ArgumentNullException(nameof(preferences));
            Name = name;
            HasDefaultValue = !IsNullOrTypeDefault(value);
            if (HasDefaultValue && IsNullOrTypeDefault(Value))
                Value = HasDefaultValue ? new List<T>(value) : new List<T>();
        }

        public PreferenceDataType DataType { get; private set; } = PreferenceDataType.Collection;

        public bool Visible { get; private set; } = true;

        public bool HasDefaultValue { get; }

        public bool HasValueSet => HasDefaultValue || !IsNullOrTypeDefault(Value);

        public bool Complete => Visible && HasValueSet;

        public string Name { get; }

        public IList<T> Value
        {
            get => provider[Name] is IList<T> result ? result : default;
            set => provider[Name] = value;
        }

        public Type ValueType => typeof(List<T>);

        public string ToView()
        {
            throw new NotImplementedException();
        }

        public void FromView(string line)
        {
            throw new NotImplementedException();
        }

        public ListPreference<T> ToggleVisibility()
        {
            Visible = !Visible;
            return this;
        }

        public ListPreference<T> UpdateDataType(PreferenceDataType dataType)
        {
            DataType = dataType;
            return this;
        }

        public static ListPreference<T> operator +(ListPreference<T> preference, T input)
        {
            preference.Value.Add(input);
            return preference;
        }

        public static ListPreference<T> operator +(ListPreference<T> preference, IEnumerable<T> args)
        {
            var list = preference.Value;
            foreach (var arg in args) list.Add(arg);
            return preference;
        }

        public static ListPreference<T> operator -(ListPreference<T> preference, T input)
        {
            preference.Value.Remove(input);
            return preference;
        }

        public static ListPreference<T> operator -(ListPreference<T> preference, IEnumerable<T> args)
        {
            var list = preference.Value;
            foreach (var arg in args) list.Remove(arg);
            return preference;
        }
    }
}