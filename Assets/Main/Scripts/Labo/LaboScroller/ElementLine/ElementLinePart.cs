using System.Collections.Generic;
using UniRx.Async;

namespace DM
{
    class ElementLinePart : UIPart
    {
        private readonly ElementLineCellView m_ElementLineCellView;

        public ElementLinePart(ElementLineCellView elementLineCellView)
            : base(elementLineCellView.transform)
        {
            m_ElementLineCellView = elementLineCellView;
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