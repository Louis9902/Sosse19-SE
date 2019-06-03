namespace TinyTasksKit.Worker.Preferences
{
    public interface IPreference
    {
        bool HasDefaultValue { get; }
        
        bool IsHidden { get; }
        
        bool IsSatisfied { get; }
        
        string Name { get; }

        string ToDisplayString();

        void FromDisplayString(string line);
    }
}