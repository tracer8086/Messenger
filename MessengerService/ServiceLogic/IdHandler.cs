using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System;

namespace MessengerService.ServiceLogic
{
    [Synchronization]
    public class IdHandler : ContextBoundObject
    {
        private readonly HashSet<uint> _freeIDs;
        private uint _amount;

        public uint MaxCount { get; }
        public bool HasFreeId => _amount > 0;

        public IdHandler(uint maxCount)
        {
            _freeIDs = new HashSet<uint>();
            MaxCount = maxCount;
            _amount = MaxCount;

            for (uint i = 0; i < MaxCount; i++)
                _freeIDs.Add(i);
        }

        public uint GetId()
        {
            var freeId = _freeIDs.First();

            _freeIDs.Remove(freeId);
            _amount--;

            return freeId;
        }

        public bool TryGetId(out uint freeId)
        {
            freeId = 0;

            if (!HasFreeId)
                return false;

            freeId = GetId();

            return true;
        }

        public void ReleaseId(uint releasedId)
        {
            if (releasedId > MaxCount - 1)
                return;

            _freeIDs.Add(releasedId);
            _amount++;
        }
    }
}