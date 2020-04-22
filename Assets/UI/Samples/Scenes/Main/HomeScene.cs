using System.Collections.Generic;
using UniRx.Async;

namespace DM {
    public class HomeScene : UIBase
    {
        private int count = 0;
        private HomeSceneView m_HomeSceneView;

        public HomeScene() : base("HomeScene", EnumUIGroup.Scene)
        {
            IsScheduleUpdate = true;
        }

        public override async UniTask OnLoadedBase()
        {
            m_HomeSceneView = RootTransform.GetComponent<HomeSceneView>();
            // RootTransform.Find("Layer/ButtonCenter").gameObject.SetActive(false);
            // RootTransform.Find("Layer/ButtonBottom").gameObject.SetActive(false);
            
            List<UIPart> parts = new List<UIPart>
            {
                new HomeScroller(m_HomeSceneView.m_HomeScroller.transform),
                new HomeTabControl(m_HomeSceneView.m_TabControl.transform),
            };
            
            await UIController.Instance.YieldAttachParts(this, parts);
            m_HomeSceneView.m_TabControl.Init(852 / 5);
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
    }
}