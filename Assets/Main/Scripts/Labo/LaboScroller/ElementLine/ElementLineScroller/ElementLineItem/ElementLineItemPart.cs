using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UniRx.Async;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DM
{
    public class ElementLineItemPart : UIPart
    {
        private readonly ElementLineItemCellView m_ElementLineItemCellView;
        private LaboScrollerBase m_LaboScrollerBase;

        public ElementLineItemPart(ElementLineItemCellView elementLineItemCellView)
            : base(elementLineItemCellView.transform)
        {
            m_ElementLineItemCellView = elementLineItemCellView;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer)
        {
            m_LaboScrollerBase = targetLayer as LaboScrollerBase;
            List<UIPart> parts = new List<UIPart>()
            {
                new ButtonPart(m_ElementLineItemCellView.m_ElementButton, ClickButton),
                new DragPart(m_ElementLineItemCellView.m_ElementDragObject, Drag, BeginDrag, EndDrag),
            };

            // 追加待ち
            await UIController.Instance.YieldAttachParts(targetLayer, parts);
        }

        private void ClickButton()
        {
            m_LaboScrollerBase.ClickMixedLineItem(m_ElementLineItemCellView.dataIndex);
        }

        private void BeginDrag(PointerEventData pointerEventData)
        {
            m_ElementLineItemCellView.m_ElementDragObject.SetDraggingParent(
                m_LaboScrollerBase.m_MixedBallTabBase.m_MixedBallTabView.transform);
            m_ElementLineItemCellView.m_ElementDragObject.BeginDrag(pointerEventData);
        }

        private void Drag(PointerEventData pointerEventData)
        {
            m_ElementLineItemCellView.m_ElementDragObject.Drag(pointerEventData);
        }

        private void EndDrag(PointerEventData pointerEventData)
        {
            m_ElementLineItemCellView.m_ElementDragObject.EndDrag(pointerEventData);
        }
    }
}