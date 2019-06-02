using System.Collections;

namespace TinyTasksKit.Worker.Preferences
{
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
}