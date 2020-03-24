using DMUIFramework.Samples;
using UnityEngine;
using UnityEngine.UI;

namespace DM
{
    public class Sample14_2 : MonoBehaviour
    {
        void Start()
        {
            UIController.SetImplement(new PrefabLoader(), new Sounder(), new FadeCreator(), new LoadingCreator(), new ToastCreator());
            UIController.Instance.AddFront(new Sample14_2Scene());
        }
    }
}