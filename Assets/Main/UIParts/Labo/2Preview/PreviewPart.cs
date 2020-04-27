using EnhancedUI.EnhancedScroller;
using UniRx.Async;

namespace DM
{
    class PreviewPart : UIPart
    {
        private readonly PreviewCellView m_PreviewCellView;

        public PreviewPart(PreviewCellView previewCellView)
            : base(previewCellView.transform)
        {
            m_PreviewCellView = previewCellView;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer) { }
    }
}