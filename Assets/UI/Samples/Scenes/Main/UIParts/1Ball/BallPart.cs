using System.Collections.Generic;
using UniRx.Async;

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
                new BallScrollerPart(this, m_BallCellView)
            };

            // 追加待ち
            await UIController.Instance.YieldAttachParts(targetLayer, parts);
        }
    }
}