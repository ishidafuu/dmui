using UniRx.Async;
using UnityEngine;

namespace DM {
    class Sample14_2CellViewButton : UIPart
    {
        private readonly CellView14_2 m_cellView14_2;

        public Sample14_2CellViewButton(CellView14_2 cellView14_2, GameObject buttonObject)
            : base(buttonObject.transform)
        {
            m_cellView14_2 = cellView14_2;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer) { }

        public override bool OnClick(TouchEvent touch, UISound uiSound)
        {
            // TouchListenerを継承してGetComponentせずに済むようなクラスを作ってもいいかも
            Debug.Log($"{m_cellView14_2.someTextText.text}");
            return true;
        }
    }
}