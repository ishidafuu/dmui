using System;
using UniRx.Async;
using UnityEngine.EventSystems;

namespace DM
{
    public class DragPart : UIPart
    {
        private readonly DragObject m_DragObject;
        private readonly Action<PointerEventData> m_ActionDrag;
        private readonly Action<PointerEventData> m_ActionBeginDrag;
        private readonly Action<PointerEventData> m_ActionEndDrag;

        public DragPart(DragObject dragObject, Action<PointerEventData> actionDrag = null,
            Action<PointerEventData> actionBeginDrag = null, Action<PointerEventData> actionEndDrag = null)
            : base(dragObject.transform)
        {
            m_DragObject = dragObject;
            m_ActionDrag = actionDrag;
            m_ActionBeginDrag = actionBeginDrag;
            m_ActionEndDrag = actionEndDrag;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer) { }

        public override bool OnDrag(TouchEvent touch)
        {
            m_ActionDrag?.Invoke(touch.Pointer);
            return false;
        }

        public override bool OnBeginDrag(TouchEvent touch)
        {
            m_ActionBeginDrag?.Invoke(touch.Pointer);
            return true;
        }

        public override bool OnEndDrag(TouchEvent touch)
        {
            m_ActionEndDrag?.Invoke(touch.Pointer);
            return false;
        }
    }
}