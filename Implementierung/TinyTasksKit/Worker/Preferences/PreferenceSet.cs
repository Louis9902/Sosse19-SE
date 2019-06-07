using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using static TinyTasksKit.Worker.Preferences.PreferenceDataType;

namespace TinyTasksKit.Worker.Preferences
{
    /// <summary>
    /// Represents a collection of IPreferences.
    /// </summary>
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

        /// <summary>
        /// Will create a new ScalarPreference preference if is does not exist, otherwise it will return the cached.
        /// </summary>
        /// <param name="name">The name of the preference</param>
        /// <param name="value">The default value of the preference</param>
        /// <typeparam name="T">The type of the preference value</typeparam>
        /// <returns>The preference, never null</returns>
        public ScalarPreference<T> Preference<T>(string name, T value = default)
        {
            if (HasPreferenceCache<T>(name, out var result)) return result;

            result = new ScalarPreference<T>(provider, name, value);
            derivations[name] = result;
            return result;
        }

        /// <summary>
        /// Will create a new ListPreference preference if is does not exist, otherwise it will return the cached.
        /// </summary>
        /// <param name="name">The name of the preference</param>
        /// <param name="values">The default values of the preference</param>
        /// <typeparam name="T">The type of the preference value</typeparam>
        /// <returns>The preference, never null</returns>
        public ListPreference<T> ListPreference<T>(string name, IList<T> values = default)
        {
            if (HasPreferenceListCache<T>(name, out var result)) return result;

            result = new ListPreference<T>(provider, name, values);
            derivations[name] = result;
            return result;
        }

        /// <summary>
        /// A IEnumerable which contains all preferences of this set.
        /// </summary>
        /// <returns>The IEnumerable with all values</returns>
        public IEnumerable<IPreference> GetAll()
        {
            return derivations.Values.Cast<IPreference>();
        }

        /// <summary>
        /// A IEnumerable which contains all visible preferences of this set.
        /// </summary>
        /// <returns>The IEnumerable with all visible values</returns>
        /// <see cref="IPreference.Visible"/>
        /// <seealso cref="Preferences.ScalarPreference{T}.ToggleVisibility()"/>
        /// <seealso cref="Preferences.ListPreference{T}.ToggleVisibility()"/>
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