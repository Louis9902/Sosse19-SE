using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using WorksKit.IO;

namespace WorksKit.Worker
{
    public interface IWorker
    {
        Guid Group { get; }
        Guid Label { get; }

        Preferences Preferences { get; }

        void StartWorker();

        void AbortWorker();
    }

    public interface IPreferenceProvider
    {
        object this[object name] { get; set; }
    }

    internal class DictionaryPreferenceProvider : IPreferenceProvider
    {
        private readonly IDictionary preferences;

        public DictionaryPreferenceProvider(IDictionary map)
        {
            preferences = map;
        }

        public object this[object name]
        {
            get => preferences[name];
            set => preferences[name] = value;
        }
    }

    public class Preferences : IEnumerable, IExternalizable
    {
        private static readonly IFormatter Formatter = new BinaryFormatter();

        private IDictionary derivations = new Hashtable(); // cache for preference objects
        private IDictionary preferences = new Hashtable(); // cache for preference options

        private IPreferenceProvider provider;

        public Preferences()
        {
            provider = new DictionaryPreferenceProvider(preferences);
        }

        private bool HasPreferenceCache<T>(string name, out Preference<T> preference)
        {
            preference = derivations[name] as Preference<T>;
            return preference != null;
        }

        public Preference<T> Preference<T>(string name, T value = default, T fallback = default)
        {
            if (HasPreferenceCache<T>(name, out var result)) return result;

            result = new Preference<T>(provider, name, value, fallback);
            derivations[name] = result;
            return result;
        }

        public IEnumerator GetEnumerator()
        {
            return derivations.Values.GetEnumerator();
        }

        public void LoadExternal(Stream stream)
        {
            preferences = Formatter.Deserialize(stream) as Hashtable;
        }

        public void SaveExternal(Stream stream)
        {
            Formatter.Serialize(stream, preferences);
        }
    }

    public class Preference<T>
    {
        private readonly IPreferenceProvider provider;

        public Preference(IPreferenceProvider preferences, string name, T value, T fallback)
        {
            provider = preferences ?? throw new ArgumentNullException(nameof(preferences));
            Value = value;
            Fallback = fallback;
            Name = name;
        }

        public string Name { get; }

        public T Value
        {
            get => provider[Name] is T result ? result : Fallback;
            set => provider[Name] = value;
        }

        public T Fallback { get; }

        public static implicit operator T(Preference<T> property)
        {
            return property.Value;
        }
    }
}