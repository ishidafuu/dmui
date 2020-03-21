using MasterData;
using UnityEngine;

namespace DM
{
    public static class MasterDataDb
    {
        public static MemoryDatabase Db { get; } = new MemoryDatabase((Resources.Load("master-data") as TextAsset)?.bytes);
    }
}