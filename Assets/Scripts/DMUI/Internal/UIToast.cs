using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

namespace DM
{
    public class UIToast : UIBase
    {
        private const int TOAST_OUT_TIME = 180;
        private readonly string m_Message;
        private int m_Count;
        
        public UIToast(string path, string message) 
            : base(path, EnumUIGroup.Loading, EnumUIPreset.Toast)
        {
            m_Message = message;
            IsScheduleUpdate = true;
        }

        public override async UniTask OnLoadedBase()
        {
            Text text = RootTransform.gameObject.GetComponentInChildren<Text>();
            if (text != null)
            {
                text.text = m_Message;
            }
            else
            {
                Debug.LogError($"UIToast Notfound TextComponent");
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