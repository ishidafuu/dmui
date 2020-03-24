using MasterMemory;
using MessagePack;

namespace DM
{
    [MemoryTable("skillParameter"), MessagePackObject(true)]
    public class SkillParameter
    {
        [PrimaryKey, NonUnique]
        [SecondaryKey(0)]
        public int SkillID { get; set; }

        [SecondaryKey(0)] public int SkillLv { get; set; }
        public int Damage { get; set; }
    }
}