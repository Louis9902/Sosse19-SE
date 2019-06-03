namespace TinyTasksKit.Worker.Preferences
{
    /// <summary>
    /// For deserialization please implement a public static TryParse method.
    /// <code>public static bool TryParse(string input, out T result)</code>
    /// </summary>
    public interface IPreferencePrinter
    {
        string ToPreferenceString();
    }
}