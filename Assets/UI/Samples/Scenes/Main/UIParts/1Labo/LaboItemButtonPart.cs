using UniRx.Async;
using UnityEngine;

namespace DM {
    class LaboItemButtonPart : UIPart
    {
        private readonly LaboItemCellView m_LaboItemCellView;

        public LaboItemButtonPart(LaboItemCellView laboItemCellView, GameObject buttonObject)
            : base(buttonObject.transform)
        {
            m_LaboItemCellView = laboItemCellView;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer) { }

        public override bool OnClick(TouchEvent touch, UISound uiSound)
        {
            // TouchListenerを継承してGetComponentせずに済むようなクラスを作ってもいいかも
            Debug.Log($"{m_LaboItemCellView.someTextText.text}");
            UIController.Instance.ToastIn("みんみー");
            return true;
        }
        
        
    }
}