using UniRx.Async;

namespace DM
{
    class HomeCell0ShopPart : UIPart
    {
        private readonly ShopCellView m_ShopCellView;

        public HomeCell0ShopPart(ShopCellView shopCellView)
            : base(shopCellView.transform)
        {
            m_ShopCellView = shopCellView;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer) { }
    }
}