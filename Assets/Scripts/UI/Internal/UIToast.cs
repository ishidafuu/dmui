using UniRx.Async;
using UnityEngine.UI;

namespace DM
{
    public class UIToast : UIBase
    {
        private const int OUT_TIME = 120;
        private readonly string m_Message;
        private int m_Count;
        
        public UIToast(string path, string message) : base(path, EnumUIGroup.Loading, EnumUIPreset.SystemIndicator)
        {
            m_Message = message;
        }

        public override async UniTask OnLoadedBase()
        {
            Text textBox = RootTransform.Find("Layer/TextBox").gameObject.GetComponent<Text>();
            textBox.text = m_Message;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            if (m_Count >= OUT_TIME)
            {
                return;
            }

            m_Count++;
            if (m_Count >= OUT_TIME)
            {
                UIController.Instance.ToastOut();
            }
        }
    }
}