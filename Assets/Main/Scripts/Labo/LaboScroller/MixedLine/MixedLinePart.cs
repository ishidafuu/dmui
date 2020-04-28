using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UniRx.Async;
using UnityEngine;

namespace DM
{
    class MixedLinePart : UIPart
    {
        private readonly MixedLineCellView m_MixedLineCellView;

        public MixedLinePart(MixedLineCellView mixedLineCellView)
            : base(mixedLineCellView.transform)
        {
            m_MixedLineCellView = mixedLineCellView;
            // m_MixedLineCellView.m_HomeScrollerView = homeScrollerView;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer)
        {
            List<UIPart> parts = new List<UIPart>
            {
                new MixedLineScrollerPart(this, m_MixedLineCellView),
                // new ButtonPart(m_MixedLineCellView.m_LaboButtonView, ClickLaboButton)
            };


            // 追加待ち
            await UIController.Instance.YieldAttachParts(targetLayer, parts);
        }

        private static void ClickLaboButton()
        {
            UIController.Instance.Replace(new UIBase[] {new LaboSceneBase()});
        }
    }
}