using System;

namespace WorksKit.Worker.Preferences
{
    public class Preference<T>
    {
        private readonly IPreferenceProvider provider;

        public Preference(IPreferenceProvider preferences, string name, T value, T fallback)
        {
            provider = preferences ?? throw new ArgumentNullException(nameof(preferences));
            Name = name;
            Fallback = fallback;
            SetDefault(value);
        }

        public string Name { get; }

        public T Value
        {
            get => provider[Name] is T result ? result : Fallback;
            set => provider[Name] = value;
        }

        public T Fallback { get; }

        public bool HasValueSet => provider[Name] is T;

        private void SetDefault(T value)
        {
            if (value != null && !value.Equals(default(T))) Value = value;
        }

        public static implicit operator T(Preference<T> property)
        {
            return property.Value;
        }
    }
}