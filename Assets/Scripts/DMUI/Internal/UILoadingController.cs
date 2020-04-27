using System;
using System.Collections.Generic;

namespace DM
{
    public class UILoadingController
    {
        private UIBaseLayer m_LoadingBaseLayer;
        
        public bool IsLoading()
        {
            return (m_LoadingBaseLayer != null && m_LoadingBaseLayer.State == EnumLayerState.Active);
        }

        public void LoadingIn(UIImplements implements, List<UIBaseLayer> addingList, Action<UIBase> addFront)
        {
            if (m_LoadingBaseLayer != null)
            {
                return;
            }

            if (implements.LoadingCreator == null)
            {
                return;
            }

            UILoading loading = implements.LoadingCreator.Create();
            addFront.Invoke(loading);
            m_LoadingBaseLayer = addingList.Find(layer => layer.Base == loading);
        }

        public void LoadingOut(Action<UIBase> remove)
        {
            if (m_LoadingBaseLayer == null)
            {
                return;
            }

            remove.Invoke(m_LoadingBaseLayer.Base);
            m_LoadingBaseLayer = null;
        }
    }
}