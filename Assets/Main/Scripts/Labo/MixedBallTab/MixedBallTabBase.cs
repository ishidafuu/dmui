using System.Collections.Generic;
using UniRx.Async;

namespace DM
{
    public class MixedBallTabBase : UIBase
    {
        public MixedBallTabView m_MixedBallTabView;
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
        
        public void ChangeMixedBall(int index)
        {
            m_MixedBallTabView.ChangeMixedBall(index);
        }
    }
}