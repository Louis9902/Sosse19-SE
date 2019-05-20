using System.IO;

namespace Backupper.Common
{
    public interface IExternalizable
    {

        void SaveExternal(BinaryWriter writer);

        void LoadExternal(BinaryReader reader);

    }
}