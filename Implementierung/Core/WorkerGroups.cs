using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backupper.Core.Tasks;

namespace Backupper.Core
{
    public class WorkerGroups
    {
        private readonly Dictionary<Guid, Type> Types;

        public bool FindGroupType(Guid id, out Type clazz)
        {
            clazz = null;
            return false;
        }
        
        public static int Register(Dictionary<byte, Type> register)
        {
            return 0;
        }

    }
}
