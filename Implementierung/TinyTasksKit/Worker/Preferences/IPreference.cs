using System;

namespace TinyTasksKit.Worker.Preferences
{
    public interface IPreference
    {
        PreferenceDataType DataType { get; }
        
        bool Visible { get; }

        bool HasDefaultValue { get; }

        bool HasValueSet { get; }

        bool Complete { get; }

        string Name { get; }

        Type ValueType { get; }
        
        string ToView();

        void FromView(string line);
    }
}