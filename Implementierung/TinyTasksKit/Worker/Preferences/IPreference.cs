using System;

namespace TinyTasksKit.Worker.Preferences
{
    /// <summary>
    /// Represents a common preference.
    /// </summary>
    /// <seealso cref="PreferenceSet"/>
    public interface IPreference
    {
        PreferenceDataType DataType { get; }

        /// <summary>
        /// Returns if the preference is visible or not.
        /// </summary>
        /// <seealso cref="Preferences.ScalarPreference{T}.ToggleVisibility()"/>
        /// <seealso cref="Preferences.ListPreference{T}.ToggleVisibility()"/>
        bool Visible { get; }

        /// <summary>
        /// Returns if the preference has a default value set.
        /// </summary>
        /// <seealso cref="PreferenceSet.Preference{T}(string, T)"/>
        /// <seealso cref="PreferenceSet.ListPreference{T}(string, System.Collections.Generic.IList{T})"/>
        bool HasDefaultValue { get; }

        /// <summary>
        /// Returns if the preference has a value set or if it is empty.
        /// </summary>
        bool HasValueSet { get; }

        /// <summary>
        /// Returns if the preference is valid and complete.
        /// This is the case when is has a value set or a default value is present.
        /// </summary>
        bool Complete { get; }

        /// <summary>
        /// Returns the name of the preference.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The value type of this preference, this is more important in the implementation.
        /// </summary>
        Type ValueType { get; }

        /// <summary>
        /// Converts the value of this preference to string which can be displayed.
        /// </summary>
        /// <returns>The view representation of this preference</returns>
        /// <seealso cref="FromView(string)"/>
        string ToView();

        /// <summary>
        /// Converts the string to match the value type of this preference.
        /// The conversion will use by default a static TryParse method. This method must return a bool.
        /// An example would be, where T is the type if the value of the preference:
        /// <code>public static bool TryParse(string line, out T result)</code>
        /// This method must be located in the value type class.
        /// </summary>
        /// <param name="line">The line to parse back</param>
        void FromView(string line);
    }
}