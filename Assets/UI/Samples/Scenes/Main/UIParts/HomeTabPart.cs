using System;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UniRx.Async;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DM 
{
    public class HomeTabPart : UIPart
    {
        // 追加先のレイヤ
        private UIBase m_TargetLayer;
        private TabView m_TabView;
        private Action<int> m_ActionClick;

        public HomeTabPart(TabView tabView, Action<int> actionClick) : base(tabView.transform)
        {
            m_TabView = tabView;
            m_ActionClick = actionClick;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer)
        {
            m_TargetLayer = targetLayer;
            
            List<UIPart> parts = new List<UIPart>();
            for (int i = 0; i < m_TabView.m_ButtonViews.Length; i++)
            {
                int index = i;
                parts.Add(new ButtonPart(m_TabView.m_ButtonViews[i], () => OnClickButton(index)));
                Debug.Log($"Add ButtonPart{index}");
            }

            // 追加待ち
            await UIController.Instance.YieldAttachParts(targetLayer, parts);
        }

        // private void InitRootTransform()
        // {
        //     // UIPartの追加先を決定する
        //     Transform layer = m_TargetLayer.RootTransform.Find("Layer");
        //     RootTransform.SetParent(layer);
        //     // RootTransform.localPosition = new Vector3(0, -400, 0);
        //     RootTransform.localScale = Vector3.one;
        // }

        private void OnClickButton(int index)
        {
            Debug.Log($"OnClickButton{index}");
            m_ActionClick?.Invoke(index);
        }
        
    }
}