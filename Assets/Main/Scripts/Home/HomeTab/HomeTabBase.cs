using System.Collections.Generic;
using UniRx.Async;

namespace DM
{
    public class HomeTabBase : UIBase
    {
        private HomeScrollerBase m_HomeScrollerBase;
        private HomeTabView m_HomeTabView;

        public HomeTabBase() : base("Home/HomeTabBase", EnumUIGroup.Floater, EnumUIPreset.Header)
        {
            IsScheduleUpdate = true;
        }

        public override async UniTask OnLoadedBase()
        {
            var homeScrollerBase = UIController.Instance.GetBaseLayer(typeof(HomeScrollerBase));
            m_HomeScrollerBase = homeScrollerBase.Base as HomeScrollerBase;
            m_HomeTabView = RootTransform.GetComponent<HomeTabView>();

            List<UIPart> parts = new List<UIPart>();
            for (int i = 0; i < m_HomeTabView.m_ButtonViews.Length; i++)
            {
                int index = i;
                parts.Add(new ButtonPart(m_HomeTabView.m_ButtonViews[i], () => ClickTabButton(index)));
            }

            // 追加待ち
            await UIController.Instance.YieldAttachParts(this, parts);
        }

        public override void OnUpdate()
        {
            if (m_HomeScrollerBase.m_HomeScrollerView == null)
            {
                return;
            }

            m_HomeTabView.UpdateCursorPosition(
                m_HomeScrollerBase.m_HomeScrollerView.m_Scroller.ScrollRect.horizontalNormalizedPosition);
        }

        private void ClickTabButton(int index)
        {
            m_HomeScrollerBase.JumpToDataIndex(index);
        }

        public override bool OnClick(TouchEvent touch, UISound uiSound)
        {
            switch (touch.Listener.name)
            {
                case "LaboButton":
                {
                    UIController.Instance.Replace(new UIBase[] {new LaboSceneBase()});
                    return true;
                }
            }

            return true;
        }
    }
}