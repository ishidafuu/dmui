using System;
using System.Collections.Generic;
using UniRx.Async;

namespace DM
{
    public class HomeTabPart : UIPart
    {
        private readonly TabObject m_TabObject;
        private readonly Action<int> m_ActionClick;

        public HomeTabPart(TabObject tabObject, Action<int> actionClick) : base(tabObject.transform)
        {
            m_TabObject = tabObject;
            m_ActionClick = actionClick;
        }

        public override async UniTask OnLoadedPart(UIBase targetLayer)
        {
            List<UIPart> parts = new List<UIPart>();
            for (int i = 0; i < m_TabObject.m_ButtonViews.Length; i++)
            {
                int index = i;
                parts.Add(new ButtonPart(m_TabObject.m_ButtonViews[i], () => ClickTabButton(index)));
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