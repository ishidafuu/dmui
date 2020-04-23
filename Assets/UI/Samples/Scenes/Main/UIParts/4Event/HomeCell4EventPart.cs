using UniRx.Async;

namespace DM
{
    class HomeCell4EventPart : UIPart
    {
        private readonly HomeCell4EventView m_HomeCell4EventView;

        public HomeCell4EventPart(HomeCell4EventView homeCell4EventView)
            : base(homeCell4EventView.transform)
        {
            m_HomeCell4EventView = homeCell4EventView;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer) { }
    }
}