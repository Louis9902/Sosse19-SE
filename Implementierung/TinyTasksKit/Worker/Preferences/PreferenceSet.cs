using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using static TinyTasksKit.Worker.Preferences.PreferenceDataType;

namespace TinyTasksKit.Worker.Preferences
{
    public class PreferenceSet
    {
        private static readonly IFormatter Formatter = new BinaryFormatter();

        private IDictionary derivations = new Hashtable(); // cache for preference objects
        private IDictionary preferences = new Hashtable(); // cache for preference options

        private IPreferenceProvider provider;

        public PreferenceSet()
        {
            provider = new DictionaryPreferenceProvider(preferences);
        }

        public static bool IsNullOrTypeDefault<T>(T value)
        {
            return Equals(default(T), value);
        }

        private bool HasPreferenceCache<T>(string name, out ScalarPreference<T> preference)
        {
            preference = derivations[name] as ScalarPreference<T>;
            return preference != null;
        }

        private bool HasPreferenceListCache<T>(string name, out ListPreference<T> preference)
        {
            preference = derivations[name] as ListPreference<T>;
            return preference != null;
        }

        public IPreference this[string name] => derivations[name] as IPreference;

        public ScalarPreference<T> Preference<T>(string name, T value = default)
        {
            if (HasPreferenceCache<T>(name, out var result)) return result;

            result = new ScalarPreference<T>(provider, name, value);
            derivations[name] = result;
            return result;
        }

        public ListPreference<T> ListPreference<T>(string name, IList<T> values = default)
        {
            if (HasPreferenceListCache<T>(name, out var result)) return result;

            result = new ListPreference<T>(provider, name, values);
            derivations[name] = result;
            return result;
        }

        public IEnumerable<IPreference> GetAll()
        {
            return derivations.Values.Cast<IPreference>();
        }

        public IEnumerable<IPreference> GetVisible()
        {
            return GetAll().Where(preference => preference.Visible);
        }

        public void Load(Stream stream)
        {
            if (!(Formatter.Deserialize(stream) is Hashtable deserialize)) return;

            foreach (DictionaryEntry next in deserialize)
            {
                preferences[next.Key] = next.Value;
            }
        }

        public void Save(Stream stream)
        {
            Formatter.Serialize(stream, preferences);
        }
    }
}