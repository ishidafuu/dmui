using System;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UniRx.Async;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DM 
{
    public class ButtonPart : UIPart
    {
        private readonly ButtonObject m_ButtonObject;
        private readonly Action m_ActionClick;

        public ButtonPart(ButtonObject buttonObject, Action actionClick = null) 
            : base(buttonObject.transform)
        {
            m_ButtonObject = buttonObject;
            m_ActionClick = actionClick;
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
        public override bool OnClick(TouchEvent touch, UISound uiSound)
        {
            m_ActionClick?.Invoke();
            return true;
        }

        public override bool OnTouchUp(TouchEvent touch)
        {
            m_ButtonObject?.PointerUp(touch.Pointer);
            // Trueにするとそこで止まってOnClickが呼ばれなくなる
            return false;
        }

        public override bool OnTouchDown(TouchEvent touch)
        {
            m_ButtonObject?.PointerDown(touch.Pointer);
            return false;
        }
        
    }
}