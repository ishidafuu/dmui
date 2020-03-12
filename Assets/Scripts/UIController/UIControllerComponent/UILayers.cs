using UnityEngine;

namespace DM
{
    public class UILayers : MonoBehaviour
    {
        private void Start()
        {
            ((RectTransform)transform).sizeDelta = new Vector2(852, 0);
        }
    }
}