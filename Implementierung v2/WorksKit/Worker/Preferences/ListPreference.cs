using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WorksKit.Worker.Preferences
{
    public class ListPreference<T>
    {
        private readonly IPreferenceProvider provider;

        public ListPreference(IPreferenceProvider preferences, string name, IEnumerable<T> value)
        {
            provider = preferences ?? throw new ArgumentNullException(nameof(preferences));
            Name = name;
            SetDefaults(value);
        }

        public string Name { get; }

        public IList<T> Value
        {
            get => provider[Name] as IList<T>;
            set => provider[Name] = value;
        }
        
        public bool HasValueSet => provider[Name] is IList<T> list && list.Count > 0;

        private void SetDefaults(IEnumerable<T> values)
        {
            var result = provider[Name];

            if (result == null)
            {
                result = provider[Name] = new List<T>();
                if (values != null)
                {
                    foreach (var value in values) ((IList<T>) result).Add(value);
                }
            }

            if (!(result is IList<T>))
            {
                throw new InvalidOperationException($"provider has different type for name {Name}");
            }
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