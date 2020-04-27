using UniRx.Async;

namespace DM
{
    class BattlePart : UIPart
    {
        private readonly BattleCellView m_BattleCellView;

        public BattlePart(BattleCellView battleCellView)
            : base(battleCellView.transform)
        {
            m_BattleCellView = battleCellView;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer) { }
    }
}