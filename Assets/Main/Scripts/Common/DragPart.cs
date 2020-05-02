using System;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UniRx.Async;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

        public override async UniTask OnLoadedPart(UIBase targetLayer)
        {
            // InitRootTransform();
            // // 追加待ち
            // await UIController.Instance.YieldAttachParts(targetLayer, parts);
        }

        // private void InitRootTransform()
        // {
        //     // UIPartの追加先を決定する
        //     Transform layer = m_TargetLayer.RootTransform.Find("Layer");
        //     RootTransform.SetParent(layer);
        //     // RootTransform.localPosition = new Vector3(0, -400, 0);
        //     RootTransform.localScale = Vector3.one;
        // }
        public override bool OnDrag(TouchEvent touch)
        {
            // return base.OnDrag(touch);
            m_ActionDrag?.Invoke(touch.Pointer);
            return false;
        }

        public override bool OnBeginDrag(TouchEvent touch)
        {
            // base.OnBeginDrag(touch);
            m_ActionBeginDrag?.Invoke(touch.Pointer);
            return true;
        }

        public override bool OnEndDrag(TouchEvent touch)
        {
            // base.OnEndDrag(touch);
            m_ActionEndDrag?.Invoke(touch.Pointer);
            return true;
        }
    }
}