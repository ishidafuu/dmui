using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UniRx.Async;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DM 
{
    public class HomeTabControl : UIPart
    {
        // 追加先のレイヤ
        private UIBase m_TargetLayer;
        private HomeScrollerController m_HomeScrollerController;
        
        public HomeTabControl(Transform rootTransform) : base(rootTransform) { }

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
        
    }
}