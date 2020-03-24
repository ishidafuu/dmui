using System;
using System.Collections.Generic;

namespace DM
{
    public class UIToastController
    {
        private UIBaseLayer m_ToastBaseLayer;

        public void PlayToast(UIImplements implements, List<UIBaseLayer> addingList, Action<UIBase> addFront, 
            string message)
        {
            if (m_ToastBaseLayer != null)
            {
                return;
            }

            if (implements.ToastCreator == null)
            {
                return;
            }

            UIToast toast = implements.ToastCreator.Create(message);
            addFront.Invoke(toast);
            m_ToastBaseLayer = addingList.Find(layer => layer.Base == toast);
        }

        public void ToastOut(Action<UIBase> remove)
        {
            if (m_ToastBaseLayer == null)
            {
                return;
            }

            remove.Invoke(m_ToastBaseLayer.Base);
            m_ToastBaseLayer = null;
        }
    }
}