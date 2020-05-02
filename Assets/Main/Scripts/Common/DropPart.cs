using System;
using UniRx.Async;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DM
{
    public class DropPart : UIPart
    {
        private readonly DropObject m_DropObject;
        private readonly Action<PointerEventData> m_ActionDrop;

        public DropPart(DropObject dropObject, Action<PointerEventData> actionDrop = null)
            : base(dropObject.transform)
        {
            m_DropObject = dropObject;
            m_ActionDrop = actionDrop;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer) { }

        public override bool OnDrop(TouchEvent touch)
        {
            m_ActionDrop?.Invoke(touch.Pointer);
            return false;
        }
    }
}