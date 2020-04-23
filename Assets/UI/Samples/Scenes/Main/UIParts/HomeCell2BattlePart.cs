using UniRx.Async;

namespace DM
{
    class HomeCell2BattlePart : UIPart
    {
        private readonly HomeCell2BattleView m_HomeCell2BattleView;

        public HomeCell2BattlePart(HomeCell2BattleView homeCell2BattleView)
            : base(homeCell2BattleView.transform)
        {
            m_HomeCell2BattleView = homeCell2BattleView;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer) { }
    }
}