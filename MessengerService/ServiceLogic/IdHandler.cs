using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System;

namespace MessengerService.ServiceLogic
{
    [Synchronization]
    public class IdHandler : ContextBoundObject
    {
        private HashSet<uint> freeIDs;
        private uint amount;

        public uint MaxCount { get; }
        public bool HasFreeID => amount > 0;

        public IdHandler(uint maxCount)
        {
            freeIDs = new HashSet<uint>();
            MaxCount = maxCount;
            amount = MaxCount;

            for (uint i = 0; i < MaxCount; i++)
                freeIDs.Add(i);
        }

        public uint GetId()
        {
            uint freeID = freeIDs.First();

            freeIDs.Remove(freeID);
            amount--;

            return freeID;
        }

        public bool TryGetId(out uint freeID)
        {
            freeID = 0;

            if (!HasFreeID)
                return false;

            freeID = GetId();

            return true;
        }

        public void ReleaseID(uint releasedID)
        {
            if (releasedID > MaxCount - 1)
                return;

            freeIDs.Add(releasedID);
            amount++;
        }
    }
}