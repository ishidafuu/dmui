using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UniRx.Async;
using UnityEngine;

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
                new ButtonPart(m_ElementLineItemCellView.m_ElementButton, ClickButton)
            };

            // 追加待ち
            await UIController.Instance.YieldAttachParts(targetLayer, parts);
        }

        private void ClickButton()
        {
            m_LaboScrollerBase.ClickMixedLineItem(m_ElementLineItemCellView.dataIndex);
        }
    }
}