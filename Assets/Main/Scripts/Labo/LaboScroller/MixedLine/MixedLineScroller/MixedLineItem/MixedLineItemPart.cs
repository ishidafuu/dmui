using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UniRx.Async;
using UnityEngine;

namespace DM
{
    public class MixedLineItemPart : UIPart
    {
        private readonly MixedLineItemCellView m_MixedLineItemCellView;
        private LaboScrollerBase m_LaboScrollerBase;
        
        public MixedLineItemPart(MixedLineItemCellView mixedLineItemCellView)
            : base(mixedLineItemCellView.transform)
        {
            m_MixedLineItemCellView = mixedLineItemCellView;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer)
        {
            m_LaboScrollerBase = targetLayer as LaboScrollerBase;
            List<UIPart> parts = new List<UIPart>()
            {
                new ButtonPart(m_MixedLineItemCellView.m_MixedBallButton, ClickButton)
            };

            // 追加待ち
            await UIController.Instance.YieldAttachParts(targetLayer, parts);
        }

        private void ClickButton()
        {
            m_LaboScrollerBase.ClickMixedLineItem(m_MixedLineItemCellView.dataIndex);
        }
    }
}