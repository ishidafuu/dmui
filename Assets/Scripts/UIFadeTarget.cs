using System.Collections.Generic;

namespace DM
{
    internal static class UIFadeTarget
    {
        public static readonly List<UIGroup> s_Groups = new List<UIGroup>()
        {
            UIGroup.Floater,
            UIGroup.MainScene,
            UIGroup.View3D,
        };
    }
}