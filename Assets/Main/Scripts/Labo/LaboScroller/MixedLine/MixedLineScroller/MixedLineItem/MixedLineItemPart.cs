using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UniRx.Async;
using UnityEngine;

namespace DM
{
    public class MixedLineItemPart : UIPart
    {
        private UIBase m_TargetLayer;
        private readonly MixedLineItemCellView m_MixedLineItemCellView;

        public MixedLineItemPart(MixedLineItemCellView mixedLineItemCellView)
            : base(mixedLineItemCellView.transform)
        {
            m_MixedLineItemCellView = mixedLineItemCellView;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer)
        {
            m_TargetLayer = targetLayer;
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