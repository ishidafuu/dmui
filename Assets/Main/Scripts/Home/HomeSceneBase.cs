﻿using System.Collections.Generic;
using UniRx.Async;

namespace DM
{
    public class HomeSceneBase : UIBase
    {
        private int count = 0;
        private HomeSceneView m_HomeSceneView;

        public HomeSceneBase() : base("Home/HomeScene", EnumUIGroup.Scene)
        {
            IsScheduleUpdate = true;
        }

        public override async UniTask OnLoadedBase()
        {
            m_HomeSceneView = RootTransform.GetComponent<HomeSceneView>();

            List<UIPart> parts = new List<UIPart>
            {
                new HomeScrollerPart(m_HomeSceneView),
            };

            await UIController.Instance.YieldAttachParts(this, parts);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (UIController.Instance.IsLoading())
            {
                count++;
                if (count > 120)
                {
                    UIController.Instance.LoadingOut();
                }
            }
            else
            {
                count = 0;
            }
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