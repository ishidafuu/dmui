using System.Collections.Generic;
using UniRx.Async;

namespace DM
{
    public class PreviewFieldBase : UIBase
    {
        private PreviewFieldView m_PreviewFieldView;
        public PreviewFieldBase() : base("Labo/PreviewFieldBase", EnumUIGroup.MainScene)
        {
            // IsScheduleUpdate = true;
        }

        public override async UniTask OnLoadedBase()
        {
            // var laboScrollerLayer = UIController.Instance.GetBaseLayer(typeof(LaboScrollerBase));
            m_PreviewFieldView = RootTransform.GetComponent<PreviewFieldView>();
            List<UIPart> parts = new List<UIPart>();

            // 追加待ち
            await UIController.Instance.YieldAttachParts(this, parts);
        }
        
    }
}