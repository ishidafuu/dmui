using System.Collections.Generic;
using UniRx.Async;

namespace DM
{
    public class MixedBallTabBase : UIBase
    {
        private MixedBallTabView m_MixedBallTabView;
        private LaboScrollerBase m_LaboScrollerBase;

        public MixedBallTabBase() : base("Labo/MixedBallTabBase", EnumUIGroup.Floater, EnumUIPreset.Frame)
        {
            IsScheduleUpdate = true;
        }

        public override async UniTask OnLoadedBase()
        {
            var laboScrollerBase = UIController.Instance.GetBaseLayer(typeof(LaboScrollerBase));
            m_LaboScrollerBase = laboScrollerBase.Base as LaboScrollerBase;
            m_MixedBallTabView = RootTransform.GetComponent<MixedBallTabView>();

            List<UIPart> parts = new List<UIPart>();
            for (int i = 0; i < m_MixedBallTabView.m_ButtonViews.Length; i++)
            {
                int index = i;
                parts.Add(new MixedBallItemPart(m_MixedBallTabView.m_ButtonViews[i], () => ClickTabButton(index)));
            }

            // 追加待ち
            await UIController.Instance.YieldAttachParts(this, parts);
        }

        // public override void OnUpdate()
        // {
        //     if (m_LaboScrollerBase.m_LaboScrollerView == null)
        //     {
        //         return;
        //     }
        //
        //     m_MixedBallTabView.UpdateCursorPosition(
        //         m_LaboScrollerBase.m_LaboScrollerView.m_Scroller.ScrollRect.horizontalNormalizedPosition);
        // }

        private void ClickTabButton(int index)
        {
            // m_LaboScrollerBase.JumpToDataIndex(index);
        }

        public override bool OnClick(TouchEvent touch, UISound uiSound)
        {
            switch (touch.Listener.name)
            {
                case "LaboButton":
                {
                    // UIController.Instance.Replace(new UIBase[] {new LaboSceneBase()});
                    return true;
                }
            }

            return true;
        }
    }
}