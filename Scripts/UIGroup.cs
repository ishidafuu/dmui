using System.Collections.Generic;

namespace DM
{
    internal static class UIBackAble
    {
        public static readonly List<UIGroup> s_Groups = new List<UIGroup>()
        {
            UIGroup.Dialog,
            UIGroup.Scene,
            UIGroup.MainScene,
            UIGroup.View3D,
        };

        public static void Sort()
        {
            s_Groups.Sort((x, y) => y - x);
        }
    }

    internal static class UIFadeTarget
    {
        public static readonly List<UIGroup> s_Groups = new List<UIGroup>()
        {
            UIGroup.Floater,
            UIGroup.MainScene,
            UIGroup.View3D,
        };
    }

    internal static class UIFadeThreshold
    {
        public static readonly Dictionary<UIGroup, int> s_Groups = new Dictionary<UIGroup, int>()
        {
            {UIGroup.Scene, 1},
        };
    }
}