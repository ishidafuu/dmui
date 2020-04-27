using UniRx.Async;

namespace DM
{
    class EventPart : UIPart
    {
        private readonly EventCellView m_EventCellView;

        public EventPart(EventCellView eventCellView)
            : base(eventCellView.transform)
        {
            m_EventCellView = eventCellView;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer) { }
    }
}