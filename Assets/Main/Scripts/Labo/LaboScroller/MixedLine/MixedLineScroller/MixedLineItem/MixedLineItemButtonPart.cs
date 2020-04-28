using UniRx.Async;
using UnityEngine;

namespace DM
{
    class MixedLineItemButtonPart : UIPart
    {
        private readonly BallItemCellView m_BallItemCellView;

        public MixedLineItemButtonPart(BallItemCellView ballItemCellView, GameObject buttonObject)
            : base(buttonObject.transform)
        {
            m_BallItemCellView = ballItemCellView;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer) { }

        public override bool OnClick(TouchEvent touch, UISound uiSound)
        {
            // TouchListenerを継承してGetComponentせずに済むようなクラスを作ってもいいかも
            Debug.Log($"{m_BallItemCellView.someTextText.text}");
            UIController.Instance.ToastIn("みんみー");
            return true;
        }
    }
}