using System.Collections.Generic;
using UniRx.Async;

namespace DM
{
    class BattlePart : UIPart
    {
        private readonly BattleCellView m_BattleCellView;

        public BattlePart(BattleCellView battleCellView)
            : base(battleCellView.transform)
        {
            m_BattleCellView = battleCellView;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer)
        {
            List<UIPart> parts = new List<UIPart>
            {
                new ButtonPart(m_BattleCellView.m_LaboButtonObject, ClickLaboButton)
            };

            // 追加待ち
            await UIController.Instance.YieldAttachParts(targetLayer, parts);
        }
        
        private static void ClickLaboButton()
        {
            UIController.Instance.Replace(new UIBase[]
            {
                new PreviewFieldBase(),             
                new LaboScrollerBase(), 
                new MixedBallTabBase(),
            });
        }
    }
}