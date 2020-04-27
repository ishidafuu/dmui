using System;
using System.Collections.Generic;
using UniRx.Async;

namespace DM
{
    public class HomeTabPart : UIPart
    {
        private readonly TabView m_TabView;
        private readonly Action<int> m_ActionClick;

        public HomeTabPart(TabView tabView, Action<int> actionClick) : base(tabView.transform)
        {
            m_TabView = tabView;
            m_ActionClick = actionClick;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer)
        {
            List<UIPart> parts = new List<UIPart>();
            for (int i = 0; i < m_TabView.m_ButtonViews.Length; i++)
            {
                int index = i;
                parts.Add(new ButtonPart(m_TabView.m_ButtonViews[i], () => ClickTabButton(index)));
            }

            // 追加待ち
            await UIController.Instance.YieldAttachParts(targetLayer, parts);
        }

        private void ClickTabButton(int index)
        {
            m_ActionClick?.Invoke(index);
        }
    }
}