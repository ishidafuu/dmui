using System.Collections.Generic;

namespace DM
{
    public class StateFlags
    {
        public readonly bool m_IsTouchable;
        public readonly bool m_IsVisible;

        private StateFlags(bool isTouchable, bool isIsVisible)
        {
            m_IsTouchable = isTouchable;
            m_IsVisible = isIsVisible;
        }

        public static readonly Dictionary<BaseLayerState, StateFlags> s_Map = new Dictionary<BaseLayerState, StateFlags>()
        {
            {BaseLayerState.None, new StateFlags(false, false)},
            {BaseLayerState.InFading, new StateFlags(false, false)},
            {BaseLayerState.Loading, new StateFlags(false, false)},
            {BaseLayerState.Adding, new StateFlags(false, false)},
            {BaseLayerState.InAnimation, new StateFlags(false, true)},
            {BaseLayerState.Active, new StateFlags(true, true)},
            {BaseLayerState.OutAnimation, new StateFlags(false, true)},
            {BaseLayerState.OutFading, new StateFlags(false, true)},
            {BaseLayerState.UselessLoading, new StateFlags(false, false)},
            {BaseLayerState.Removing, new StateFlags(false, false)},
        };
    }
}