using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

namespace DM
{
    public class UIToast : UIBase
    {
        private const int TOAST_OUT_TIME = 120;
        private readonly string m_Message;
        private readonly string m_TextComponent;
        private int m_Count;
        
        public UIToast(string path, string textComponent, string message) 
            : base(path, EnumUIGroup.Loading, EnumUIPreset.SystemIndicator)
        {
            m_Message = message;
            m_TextComponent = textComponent;
        }

        public override async UniTask OnLoadedBase()
        {
            Text text = RootTransform.Find(m_TextComponent).gameObject.GetComponent<Text>();
            if (text != null)
            {
                text.text = m_Message;
            }
            else
            {
                Debug.LogError($"UIToast Notfound {m_TextComponent}");
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            if (m_Count >= TOAST_OUT_TIME)
            {
                return;
            }

            m_Count++;
            if (m_Count >= TOAST_OUT_TIME)
            {
                UIController.Instance.ToastOut();
            }
        }
    }
}