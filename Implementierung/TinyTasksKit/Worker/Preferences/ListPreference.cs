using System;
using System.Collections.Generic;

namespace TinyTasksKit.Worker.Preferences
{
    public class ListPreference<T> : IPreference
    {
        private readonly IPreferenceProvider provider;

        public ListPreference(IPreferenceProvider preferences, string name, IList<T> value)
        {
            provider = preferences ?? throw new ArgumentNullException(nameof(preferences));
            Name = name;
            HasDefaultValue = !IsDefaultOrNull(value);
            if (IsDefaultOrNull(Value)) Value = HasDefaultValue ? new List<T>(value) : new List<T>();
        }

        public bool HasDefaultValue { get; }
        public bool IsHidden { get; private set; }

        public string Name { get; }

        public IList<T> Value
        {
            get => provider[Name] as IList<T>;
            set => provider[Name] = value;
        }

        public bool IsSatisfied => HasDefaultValue || !IsDefaultOrNull(Value);

        private static bool IsDefaultOrNull<TV>(TV value)
        {
            return Equals(default(TV), value);
        }

        public ListPreference<T> ToggleHidden()
        {
            IsHidden = !IsHidden;
            return this;
        }

        public string ToDisplayString()
        {
            // ToDo: implement list display (write)
            return null;
        }

        public void FromDisplayString(string line)
        {
            // ToDo: implement list display (read)
            return;
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