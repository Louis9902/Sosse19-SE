using System.IO;

namespace WorksKit.IO
{
    public interface IExternalizable
    {
        void LoadExternal(Stream stream);

        void SaveExternal(Stream stream);
    }
}