using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UniRx.Async;
using UnityEngine;

namespace DM
{
    public class MixedLineItemPart : UIPart
    {
        private readonly MixedLineItemCellView m_MixedLineItemCellView;

        public MixedLineItemPart(MixedLineItemCellView mixedLineItemCellView)
            : base(mixedLineItemCellView.transform)
        {
            m_MixedLineItemCellView = mixedLineItemCellView;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer)
        {
            List<UIPart> parts = new List<UIPart>()
            {
                new ButtonPart(m_MixedLineItemCellView.m_MixedBallButton, ClickButton)
            };

            // 追加待ち
            await UIController.Instance.YieldAttachParts(targetLayer, parts);
        }

        private void ClickButton()
        {
            
            
            Debug.Log($"OnClick MixedLineItemPart {m_MixedLineItemCellView.GetHour()}");
        }
    }
}