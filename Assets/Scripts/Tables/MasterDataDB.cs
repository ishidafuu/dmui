using MasterData;
using UnityEngine;

namespace DM
{
    public class MasterDataDB
    {
        private static MemoryDatabase _db = new MemoryDatabase((Resources.Load("master-data") as TextAsset).bytes);
        public static MemoryDatabase DB => _db;
    }
}