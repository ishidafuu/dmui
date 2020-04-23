using UniRx.Async;

namespace DM
{
    class HomeCell0ShopPart : UIPart
    {
        private readonly HomeCell0ShopView m_HomeCell0ShopView;

        public HomeCell0ShopPart(HomeCell0ShopView homeCell0ShopView)
            : base(homeCell0ShopView.transform)
        {
            m_HomeCell0ShopView = homeCell0ShopView;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer) { }
    }
}