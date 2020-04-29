using System;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UniRx.Async;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DM 
{
    public class MixedBallItemPart : UIPart
    {
        private UIBase m_TargetLayer;
        private readonly MixedBallItemView m_MixedBallItemView;
        private readonly Action m_ActionClick;

        public MixedBallItemPart(MixedBallItemView mixedBallItemView, Action actionClick = null) 
            : base(mixedBallItemView.transform)
        {
            m_MixedBallItemView = mixedBallItemView;
            m_ActionClick = actionClick;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer)
        {
            m_TargetLayer = targetLayer;
            
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
            return base.OnClick(touch, uiSound);
        }

        public override bool OnTouchUp(TouchEvent touch)
        {
            m_MixedBallItemView?.PointerUp(touch.Pointer);
            return base.OnTouchUp(touch);
        }

        public override bool OnTouchDown(TouchEvent touch)
        {
            m_MixedBallItemView?.PointerDown(touch.Pointer);
            return base.OnTouchDown(touch);
        }
        
    }
}