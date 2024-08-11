using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navislamia.Game.Utilities
{
    public class HandleUtility
    {

        public HashSet<uint> ClaimedHandles = new HashSet<uint>();
        public static HashSet<uint> CachedHandles = new HashSet<uint>();

        public uint GenerateHandle()
        {
            lock (ClaimedHandles)
            {
                if (ClaimedHandles.Count == 0)
                {
                    ClaimedHandles.Add(0);

                    return 0;
                }

                uint maxId = ClaimedHandles.Max() + 1;

                ClaimedHandles.Add(maxId);

                return maxId;
            }
        }

        public void ReleaseHandle(uint handle)
        {
            lock (ClaimedHandles) 
            {
                ClaimedHandles.Remove(handle);
            }

            lock (CachedHandles)
            {
                CachedHandles.Add(handle);
            }
        }

    }
}
