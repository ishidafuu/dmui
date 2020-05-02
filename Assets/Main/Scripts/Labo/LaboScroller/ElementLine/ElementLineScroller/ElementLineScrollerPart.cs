using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UniRx.Async;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DM
{
    public class ElementLineScrollerPart : UIPart
    {
        // 追加先のレイヤ
        private readonly UIPart m_TargetPart;
        private readonly ElementLineScrollerView m_ElementLineScrollerView;
        private readonly LaboScrollerView m_LaboScrollerView;

        enum ScrollAngle
        {
            None,
            X,
            Y,
        }

        private ScrollAngle m_ScrollAngle = ScrollAngle.None;

        public ElementLineScrollerPart(UIPart targetPart, ElementLineCellView elementLineCellView)
            : base(elementLineCellView.m_ElementLineScrollerView.transform)
        {
            m_TargetPart = targetPart;
            m_ElementLineScrollerView = elementLineCellView.m_ElementLineScrollerView;
            m_LaboScrollerView = elementLineCellView.m_LaboScrollerView;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer)
        {
            InitElementLineScrollerController();
        }

        private void InitElementLineScrollerController()
        {
            m_ElementLineScrollerView.Init(CellViewInstantiated, ScrollerDrag, ScrollerBeginDrag, ScrollerEndDrag);
        }

        // 新規セルビュー追加時デリゲート
        private void CellViewInstantiated(EnhancedScroller scroller, EnhancedScrollerCellView cellView)
        {
            List<UIPart> parts = new List<UIPart>
            {                                
                new ElementLineItemPart(cellView as ElementLineItemCellView)
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
                    m_ElementLineScrollerView.m_Scroller.ScrollRect.OnDrag(data);
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
                    m_ElementLineScrollerView.m_Scroller.ScrollRect.enabled = true;
                    m_ElementLineScrollerView.m_Scroller.ScrollRect.OnBeginDrag(data);
                }
                else if (Mathf.Abs(data.position.x - data.pressPosition.x) > 1)
                {
                    m_ScrollAngle = ScrollAngle.X;
                    m_ElementLineScrollerView.m_Scroller.ScrollRect.enabled = false;
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
                    m_ElementLineScrollerView.m_Scroller.ScrollRect.OnEndDrag(data);
                    break;
            }

            m_ScrollAngle = ScrollAngle.None;
        }
    }
}