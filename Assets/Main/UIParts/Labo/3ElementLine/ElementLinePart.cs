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

        public override async UniTask OnLoadedPart(UIBase targetLayer) { }
    }
}