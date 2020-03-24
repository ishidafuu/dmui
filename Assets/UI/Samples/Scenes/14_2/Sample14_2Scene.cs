using System.Collections.Generic;
using UniRx.Async;

namespace DM {
    class Sample14_2Scene : UIBase
    {
        private int count = 0;

        public Sample14_2Scene() : base("UISceneA_2", EnumUIGroup.Scene)
        {
            IsScheduleUpdate = true;
        }

        public override async UniTask OnLoadedBase()
        {
            RootTransform.Find("Layer/ButtonTop").gameObject.SetActive(false);
            RootTransform.Find("Layer/ButtonCenter").gameObject.SetActive(false);
            RootTransform.Find("Layer/ButtonBottom").gameObject.SetActive(false);
            List<UIPart> parts = new List<UIPart>
            {
                new Sample14_2Scroller()
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
    }
}