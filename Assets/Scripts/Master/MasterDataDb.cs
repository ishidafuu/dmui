using MasterData;
using UnityEngine;

namespace DM
{
    public static class MasterDataDb
    {
        public const string MASTER_RESOURCE_PATH = "master-data";

        public static MemoryDatabase Db { get; }
            = new MemoryDatabase((Resources.Load(MASTER_RESOURCE_PATH) as TextAsset)?.bytes);
    }
}