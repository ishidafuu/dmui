using System.Collections.Generic;

namespace DM
{
    // このグループのレイヤが追加／削除されるとフェードが発生
    
    internal static class UIFadeTarget
    {
        public static readonly List<EnumUIGroup> s_Groups = new List<EnumUIGroup>()
        {
            EnumUIGroup.Floater,
            EnumUIGroup.MainScene,
            EnumUIGroup.View3D,
        };
    }
}