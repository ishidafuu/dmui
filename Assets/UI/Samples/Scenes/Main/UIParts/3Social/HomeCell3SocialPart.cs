using UniRx.Async;

namespace DM
{
    class HomeCell3SocialPart : UIPart
    {
        private readonly HomeCell3SocialView m_HomeCell3SocialView;

        public HomeCell3SocialPart(HomeCell3SocialView homeCell3SocialView)
            : base(homeCell3SocialView.transform)
        {
            m_HomeCell3SocialView = homeCell3SocialView;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer) { }
    }
}