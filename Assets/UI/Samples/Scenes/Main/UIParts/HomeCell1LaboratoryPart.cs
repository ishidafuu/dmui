using UniRx.Async;

namespace DM
{
    class HomeCell1LaboratoryPart : UIPart
    {
        private readonly HomeCell1LaboratoryView m_HomeCell1LaboratoryView;

        public HomeCell1LaboratoryPart(HomeCell1LaboratoryView homeCell1LaboratoryView)
            : base(homeCell1LaboratoryView.transform)
        {
            m_HomeCell1LaboratoryView = homeCell1LaboratoryView;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer) { }
    }
}