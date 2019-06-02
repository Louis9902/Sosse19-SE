namespace TinyTasksKit.Worker.Preferences
{
    public interface IPreferenceProvider
    {
        object this[object name] { get; set; }
    }
}