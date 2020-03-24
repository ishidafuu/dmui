using UniRx.Async;
using UnityEngine.UI;

namespace DM
{
    public class UIToast : UIBase
    {
        private readonly string m_Message;

        public UIToast(string path, string message) : base(path, EnumUIGroup.Loading, EnumUIPreset.SystemIndicator)
        {
            m_Message = message;
        }

        public override async UniTask OnLoadedBase()
        {
            Text textBox = RootTransform.Find("Layer/TextBox").gameObject.GetComponent<Text>();
            textBox.text = m_Message;
        }
    }
}