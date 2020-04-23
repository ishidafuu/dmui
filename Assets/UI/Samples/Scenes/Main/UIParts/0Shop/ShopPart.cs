using UniRx.Async;

namespace DM
{
    class ShopPart : UIPart
    {
        private readonly ShopCellView m_ShopCellView;

        public ShopPart(ShopCellView shopCellView)
            : base(shopCellView.transform)
        {
            m_ShopCellView = shopCellView;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer) { }
    }
}