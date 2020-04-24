using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UniRx.Async;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DM
{
    public class LaboScrollerPart : UIPart
    {
        // 追加先のレイヤ
        private UIBase m_TargetLayer;
        private readonly UIPart m_TargetPart;
        private readonly LaboScrollerController m_LaboScrollerController;
        private readonly LaboScrollerView m_LaboScrollerView;
        private readonly HomeScrollerView m_HomeScrollerView;

        enum ScrollAngle
        {
            None,
            X,
            Y,
        }

        private ScrollAngle m_ScrollAngle = ScrollAngle.None;

        // public LaboScrollerPart() : base("HomeScroller") { }
        public LaboScrollerPart(UIPart targetPart,LaboCellView laboCellView)
            : base(laboCellView.m_LaboScrollerView.transform)
        {
            m_TargetPart = targetPart;
            m_LaboScrollerView = laboCellView.m_LaboScrollerView;
            m_LaboScrollerController = laboCellView.m_LaboScrollerController;
            m_HomeScrollerView = laboCellView.m_HomeScrollerView;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer)
        {
            m_TargetLayer = targetLayer;
            InitRootTransform();
            InitLaboScrollerController();

            // List<UIPart> parts = new List<UIPart>
            // {
            //     new HomeTabPart(m_LaboScrollerView.m_TabView, JumpToDataIndex)
            // };
            //
            // // 追加待ち
            // await UIController.Instance.YieldAttachParts(targetLayer, parts);
            //
            // const int FIRST_INDEX = 2;
            // m_LaboScrollerView.m_EnhancedScroller.JumpToDataIndex(FIRST_INDEX);
        }

        private void InitRootTransform()
        {
            // UIPartの追加先を決定する
            Transform layer = m_TargetPart.RootTransform.Find("Layer");
            RootTransform.SetParent(layer);
            RootTransform.localPosition = new Vector3(0, 0, 0);
            RootTransform.localScale = Vector3.one;
        }

        private void InitLaboScrollerController()
        {
            m_LaboScrollerController.Init(ScrollerDrag, ScrollerBeginDrag, ScrollerEndDrag);
            // m_LaboScrollerController.Init(CellViewInstantiated, ScrollerScrollingChanged,
            //     ScrollerBeginDrag, ScrollerEndDrag);
        }
        
        private void ScrollerDrag(EnhancedScroller scroller, PointerEventData data)
        {
            switch (m_ScrollAngle)
            {
                case ScrollAngle.X:
                    m_HomeScrollerView.m_EnhancedScroller.OnDrag(data);
                    m_HomeScrollerView.m_EnhancedScroller.ScrollRect.OnDrag(data);
                    break;
                case ScrollAngle.Y:
                    m_LaboScrollerView.m_EnhancedScroller.ScrollRect.OnDrag(data);
                    break;
            }
        }
        
        private void ScrollerBeginDrag(EnhancedScroller scroller, PointerEventData data)
        {
            if (m_ScrollAngle == ScrollAngle.None)
            {
                if (Mathf.Abs(data.position.y - data.pressPosition.y) > 1)
                {
                    m_ScrollAngle = ScrollAngle.Y;
                    m_LaboScrollerView.m_EnhancedScroller.ScrollRect.enabled = true;
                    m_LaboScrollerView.m_EnhancedScroller.ScrollRect.OnBeginDrag(data);
                }
                else if (Mathf.Abs(data.position.x - data.pressPosition.x) > 1)
                {
                    m_ScrollAngle = ScrollAngle.X;
                    m_LaboScrollerView.m_EnhancedScroller.ScrollRect.enabled = false;
                    m_HomeScrollerView.m_EnhancedScroller.OnBeginDrag(data);
                    m_HomeScrollerView.m_EnhancedScroller.ScrollRect.OnBeginDrag(data);
                }
            }
            
        }
        
        private void ScrollerEndDrag(EnhancedScroller scroller, PointerEventData data)
        {
            switch (m_ScrollAngle)
            {
                case ScrollAngle.X:
                    m_HomeScrollerView.m_EnhancedScroller.OnEndDrag(data);
                    m_HomeScrollerView.m_EnhancedScroller.ScrollRect.OnEndDrag(data);
                    break;
                case ScrollAngle.Y:
                    m_LaboScrollerView.m_EnhancedScroller.ScrollRect.OnEndDrag(data);
                    break;
            }

            m_ScrollAngle = ScrollAngle.None;
        }
    }
}