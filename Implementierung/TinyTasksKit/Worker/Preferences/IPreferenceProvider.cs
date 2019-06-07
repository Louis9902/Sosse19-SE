namespace TinyTasksKit.Worker.Preferences
{
    /// <summary>
    /// Represents a provider for preference values.
    /// This allows getting and setting them with their name.
    /// </summary>
    public interface IPreferenceProvider
    {
        object this[object name] { get; set; }
    }
}