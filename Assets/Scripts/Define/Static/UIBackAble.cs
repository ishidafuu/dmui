using System.Collections.Generic;

namespace DM
{
    internal static class UIBackAble
    {
        // 戻るによる削除対象のグループ
        // この中の最前面のグループのOnBackが呼ばれる
        public static readonly List<EnumUIGroup> s_Groups = new List<EnumUIGroup>()
        {
            EnumUIGroup.Dialog,
            EnumUIGroup.Scene,
            EnumUIGroup.MainScene,
            EnumUIGroup.View3D,
        };

        public static void Sort()
        {
            s_Groups.Sort((x, y) => y - x);
        }
    }
}