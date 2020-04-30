using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UniRx.Async;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DM
{
    public class MixedLineScrollerPart : UIPart
    {
        // 追加先のレイヤ
        private readonly UIPart m_TargetPart;
        private readonly MixedLineScrollerView m_MixedLineScrollerView;
        private readonly LaboScrollerView m_LaboScrollerView;

        enum ScrollAngle
        {
            None,
            X,
            Y,
        }

        private ScrollAngle m_ScrollAngle = ScrollAngle.None;

        public MixedLineScrollerPart(UIPart targetPart, MixedLineCellView mixedLineCellView)
            : base(mixedLineCellView.m_MixedLineScrollerView.transform)
        {
            m_TargetPart = targetPart;
            m_MixedLineScrollerView = mixedLineCellView.m_MixedLineScrollerView;
            m_LaboScrollerView = mixedLineCellView.m_LaboScrollerView;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer)
        {
            InitRootTransform();
            InitMixedLineScrollerController();
        }

        private void InitRootTransform()
        {
            // UIPartの追加先を決定する
            Transform layer = m_TargetPart.RootTransform.Find("Layer");
            RootTransform.SetParent(layer);
            RootTransform.localPosition = new Vector3(0, 0, 0);
            RootTransform.localScale = Vector3.one;
        }

        private void InitMixedLineScrollerController()
        {
            m_MixedLineScrollerView.Init(CellViewInstantiated, ScrollerDrag, ScrollerBeginDrag, ScrollerEndDrag);
        }

        // 新規セルビュー追加時デリゲート
        private void CellViewInstantiated(EnhancedScroller scroller, EnhancedScrollerCellView cellView)
        {
            List<UIPart> parts = new List<UIPart>
            {
                new MixedLineItemPart(cellView as MixedLineItemCellView)
            };

            // 即時追加
            UIController.Instance.AttachParts(TargetLayer, parts);
        }

        private void ScrollerDrag(EnhancedScroller scroller, PointerEventData data)
        {
            switch (m_ScrollAngle)
            {
                case ScrollAngle.X:
                    m_LaboScrollerView.m_Scroller.OnDrag(data);
                    m_LaboScrollerView.m_Scroller.ScrollRect.OnDrag(data);
                    break;
                case ScrollAngle.Y:
                    m_MixedLineScrollerView.m_Scroller.ScrollRect.OnDrag(data);
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
                    m_MixedLineScrollerView.m_Scroller.ScrollRect.enabled = true;
                    m_MixedLineScrollerView.m_Scroller.ScrollRect.OnBeginDrag(data);
                }
                else if (Mathf.Abs(data.position.x - data.pressPosition.x) > 1)
                {
                    m_ScrollAngle = ScrollAngle.X;
                    m_MixedLineScrollerView.m_Scroller.ScrollRect.enabled = false;
                    m_LaboScrollerView.m_Scroller.OnBeginDrag(data);
                    m_LaboScrollerView.m_Scroller.ScrollRect.OnBeginDrag(data);
                }
            }
        }

        private void ScrollerEndDrag(EnhancedScroller scroller, PointerEventData data)
        {
            switch (m_ScrollAngle)
            {
                case ScrollAngle.X:
                    m_LaboScrollerView.m_Scroller.OnEndDrag(data);
                    m_LaboScrollerView.m_Scroller.ScrollRect.OnEndDrag(data);
                    break;
                case ScrollAngle.Y:
                    m_MixedLineScrollerView.m_Scroller.ScrollRect.OnEndDrag(data);
                    break;
            }

            m_ScrollAngle = ScrollAngle.None;
        }
    }
}