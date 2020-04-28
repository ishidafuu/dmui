using UniRx.Async;

namespace DM
{
    class ElementListPart : UIPart
    {
        private readonly ElementListCellView m_ElementListCellView;

        public ElementListPart(ElementListCellView elementListCellView)
            : base(elementListCellView.transform)
        {
            m_ElementListCellView = elementListCellView;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer) { }
    }
}