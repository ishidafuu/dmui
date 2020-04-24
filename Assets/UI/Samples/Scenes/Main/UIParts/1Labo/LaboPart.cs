using System.Collections.Generic;
using UniRx.Async;

namespace DM
{
    class LaboPart : UIPart
    {
        private readonly LaboCellView m_LaboCellView;

        public LaboPart(LaboCellView laboCellView, HomeScrollerView homeScrollerView)
            : base(laboCellView.transform)
        {
            m_LaboCellView = laboCellView;
            m_LaboCellView.m_HomeScrollerView = homeScrollerView;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer)
        {
            List<UIPart> parts = new List<UIPart>
            {    
                new LaboScrollerPart(this, m_LaboCellView)
            };

            // 追加待ち
            await UIController.Instance.YieldAttachParts(targetLayer, parts);
        }
    }
}