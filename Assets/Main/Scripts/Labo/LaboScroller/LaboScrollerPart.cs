﻿using System.Collections.Generic;
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
        private readonly LaboScrollerController m_LaboScrollerController;
        private readonly LaboScrollerView m_LaboScrollerView;

        // public LaboScrollerPart() : base("HomeScroller") { }
        public LaboScrollerPart(LaboSceneView laboSceneView)
            : base(laboSceneView.m_LaboScrollerView.transform)
        {
            m_LaboScrollerView = laboSceneView.m_LaboScrollerView;
            m_LaboScrollerController = laboSceneView.m_LaboScrollerController;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer)
        {
            m_TargetLayer = targetLayer;
            InitRootTransform();
            InitLaboScrollerController();

            List<UIPart> parts = new List<UIPart>
            {
                // new HomeTabPart(m_LaboScrollerView.m_TabView, JumpToDataIndex)
            };

            // 追加待ち
            await UIController.Instance.YieldAttachParts(targetLayer, parts);

            const int FIRST_INDEX = 2;
            m_LaboScrollerView.m_EnhancedScroller.JumpToDataIndex(FIRST_INDEX);
        }

        private void InitRootTransform()
        {
            // UIPartの追加先を決定する
            Transform layer = m_TargetLayer.RootTransform.Find("Layer");
            RootTransform.SetParent(layer);
            RootTransform.localPosition = new Vector3(0, 0, 0);
            RootTransform.localScale = Vector3.one;
        }

        private void InitLaboScrollerController()
        {
            m_LaboScrollerController.Init(CellViewInstantiated, ScrollerScrollingChanged,
                ScrollerBeginDrag, ScrollerEndDrag);
        }

        // 新規セルビュー追加時デリゲート
        private void CellViewInstantiated(EnhancedScroller scroller, EnhancedScrollerCellView cellView)
        {
            UIPart part = null;
            switch (cellView.cellIdentifier)
            {
                case "MixedListCellView":
                    part = new MixedListPart(cellView as MixedListCellView);
                    break;
                case "MixedLineCellView":
                    part = new MixedLinePart(cellView as MixedLineCellView);
                    break;
                case "PreviewCellView":    
                    part = new PreviewPart(cellView as PreviewCellView);
                    break;
                case "ElementLineCellView":
                    part = new ElementLinePart(cellView as ElementLineCellView);
                    break;
                case "ElementListCellView":
                    part = new ElementListPart(cellView as ElementListCellView);
                    break;
                default:
                    Debug.LogError($"CellViewInstantiated Error cellIdentifier:{cellView.cellIdentifier}");
                    return;
            }

            List<UIPart> parts = new List<UIPart>
            {
                part
            };

            // 即時追加
            UIController.Instance.AttachParts(m_TargetLayer, parts);
        }


        private void ScrollerScrollingChanged(EnhancedScroller scroller, bool scrolling) { }

        private void ScrollerBeginDrag(EnhancedScroller scroller, PointerEventData data)
        {
            scroller.CancelTweening();
        }

        private void ScrollerEndDrag(EnhancedScroller scroller, PointerEventData data)
        {
            // ScrollRectのInertiaはFalseにしておく
            // TweenはEaseInQuad

            const float BORDER = 0.01f;
            int index = (int)(scroller.ScrollPosition / m_LaboScrollerController.CellSize + 0.5f);
            bool isPrevShift = (int)scroller.ScrollPosition % (int)m_LaboScrollerController.CellSize
                               > m_LaboScrollerController.CellSize / 2;

            if (data.delta.x < -BORDER && !isPrevShift)
            {
                index += 1;
            }
            else if (data.delta.x > BORDER && isPrevShift)
            {
                index -= 1;
            }

            JumpToDataIndex(index);
        }

        private void JumpToDataIndex(int index)
        {
            if (m_LaboScrollerView.m_EnhancedScroller.IsTweening)
            {
                return;
            }

            m_LaboScrollerView.m_EnhancedScroller.JumpToDataIndex(index, 0, 0, true,
                EnhancedScroller.TweenType.easeOutQuart, 0.5f,
                () => m_LaboScrollerView.m_EnhancedScroller.Velocity = Vector2.zero);
        }
    }
}