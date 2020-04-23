using UniRx.Async;

namespace DM
{
    class LaboratoryPart : UIPart
    {
        private readonly LaboratoryCellView m_LaboratoryCellView;

        public LaboratoryPart(LaboratoryCellView laboratoryCellView)
            : base(laboratoryCellView.transform)
        {
            m_LaboratoryCellView = laboratoryCellView;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer) { }
    }
}