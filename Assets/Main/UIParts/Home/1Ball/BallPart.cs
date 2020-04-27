using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UniRx.Async;
using UnityEngine;

namespace DM
{
    class BallPart : UIPart
    {
        private readonly BallCellView m_BallCellView;

        public BallPart(BallCellView ballCellView, HomeScrollerView homeScrollerView)
            : base(ballCellView.transform)
        {
            m_BallCellView = ballCellView;
            m_BallCellView.m_HomeScrollerView = homeScrollerView;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer)
        {
            List<UIPart> parts = new List<UIPart>
            {
                new BallScrollerPart(this, m_BallCellView),
                new ButtonPart(m_BallCellView.m_LaboButtonView, ClickLaboButton)
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