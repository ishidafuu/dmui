using System.Collections.Generic;

namespace DM
{
    internal static class UIFadeThreshold
    {
        public static readonly Dictionary<UIGroup, int> s_Groups = new Dictionary<UIGroup, int>()
        {
            {UIGroup.Scene, 1},
        };
    }
}