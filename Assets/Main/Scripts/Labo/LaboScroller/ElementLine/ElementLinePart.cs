using System.Collections.Generic;
using UniRx.Async;

namespace DM
{
    public class ElementLinePart : UIPart
    {
        private readonly ElementLineCellView m_ElementLineCellView;

        public ElementLinePart(ElementLineCellView elementLineCellView, LaboScrollerView laboScrollerView,
            MixedBallTabView mixedBallTabView)
            : base(elementLineCellView.transform)
        {
            m_ElementLineCellView = elementLineCellView;
            m_ElementLineCellView.m_LaboScrollerView = laboScrollerView;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer)
        {
            List<UIPart> parts = new List<UIPart>
            {
                new ElementLineScrollerPart(this, m_ElementLineCellView),
                // new ButtonPart(m_MixedLineCellView.m_LaboButtonView, ClickLaboButton)
            };


            // 追加待ち
            await UIController.Instance.YieldAttachParts(targetLayer, parts);
        }
    }
}