namespace WorksKit.Worker.Preferences
{
    public interface IPreferenceProvider
    {
        object this[object name] { get; set; }
    }
}