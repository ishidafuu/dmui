using UniRx.Async;

namespace DM
{
    class SocialPart : UIPart
    {
        private readonly SocialCellView m_SocialCellView;

        public SocialPart(SocialCellView socialCellView)
            : base(socialCellView.transform)
        {
            m_SocialCellView = socialCellView;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer) { }
    }
}