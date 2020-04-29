using DMUIFramework.Samples;
using UnityEngine;

namespace DM
{
    public class DMUICanvas : MonoBehaviour
    {
        private void Start()
        {
            UIController.SetImplement(new PrefabLoader(), new Sounder(), new FadeCreator(), new LoadingCreator(), new ToastCreator());
            UIController.Instance.AddFront(new HomeScrollerBase());
            UIController.Instance.AddFront(new HomeTabBase());
        }
    }
}
