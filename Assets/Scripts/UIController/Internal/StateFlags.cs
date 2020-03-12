using System.Collections.Generic;

namespace DM
{
    public class StateFlags
    {
        private readonly bool m_IsTouchable;
        private readonly bool m_IsVisible;

        private StateFlags(bool isTouchable, bool isIsVisible)
        {
            m_IsTouchable = isTouchable;
            m_IsVisible = isIsVisible;
        }

        private static readonly Dictionary<EnumLayerState, StateFlags> s_Map =
            new Dictionary<EnumLayerState, StateFlags>()
            {
                {EnumLayerState.None, new StateFlags(false, false)},
                {EnumLayerState.InFading, new StateFlags(false, false)},
                {EnumLayerState.Loading, new StateFlags(false, false)},
                {EnumLayerState.Adding, new StateFlags(false, false)},
                {EnumLayerState.InAnimation, new StateFlags(false, true)},
                {EnumLayerState.Active, new StateFlags(true, true)},
                {EnumLayerState.OutAnimation, new StateFlags(false, true)},
                {EnumLayerState.OutFading, new StateFlags(false, true)},
                {EnumLayerState.UselessLoading, new StateFlags(false, false)},
                {EnumLayerState.Removing, new StateFlags(false, false)},
            };

        public static bool IsVisible(EnumLayerState state) => s_Map[state].m_IsVisible;
        public static bool IsTouchable(EnumLayerState state) => s_Map[state].m_IsTouchable;
    }
}