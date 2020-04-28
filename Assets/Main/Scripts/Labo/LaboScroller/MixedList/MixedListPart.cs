using UniRx.Async;

namespace DM
{
    class MixedListPart : UIPart
    {
        private readonly MixedListCellView m_MixedListCellView;

        public MixedListPart(MixedListCellView mixedListCellView)
            : base(mixedListCellView.transform)
        {
            m_MixedListCellView = mixedListCellView;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer) { }
    }
}