using System.Collections.Generic;
using UniRx.Async;

namespace DM {
    public class LaboSceneBase : UIBase
    {
        private int count = 0;
        private LaboSceneView m_LaboSceneView;

        public LaboSceneBase() : base("LaboScene", EnumUIGroup.Scene)
        {
            IsScheduleUpdate = true;
        }

        public override async UniTask OnLoadedBase()
        {
            m_LaboSceneView = RootTransform.GetComponent<LaboSceneView>();

            List<UIPart> parts = new List<UIPart>
            {
                new LaboScrollerPart(m_LaboSceneView),
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