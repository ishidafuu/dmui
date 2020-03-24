using MasterData;
using UnityEngine;

namespace DM
{
    public static class MasterDataDb
    {
        public static readonly string m_MasterData = "master-data";
        public static MemoryDatabase Db { get; } 
            = new MemoryDatabase((Resources.Load(m_MasterData) as TextAsset)?.bytes);
    }
}