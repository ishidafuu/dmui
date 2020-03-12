using System.Collections.Generic;

namespace DM
{
    // この数値以下のレイヤ数で追加削除が行われたときのみフェードが発生
    internal static class UIFadeThreshold
    {
        public static readonly Dictionary<EnumUIGroup, int> s_Groups = new Dictionary<EnumUIGroup, int>()
        {
            {EnumUIGroup.Scene, 1},
        };
    }
}