using System;

namespace TinyTasksKit.Worker.Preferences
{
    public class Preference<T>
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
        
        private static bool IsDefaultOrNull<TV>(TV value)
        {
            return Equals(default(TV), value);
        }

        public Preference<T> MakeHidden()
        {
            IsHidden = !IsHidden;
            return this;
        }

        public static implicit operator T(Preference<T> property)
        {
            return property.Value;
        }
    }
}