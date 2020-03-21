using UnityEditor;
using UnityEngine;

namespace DM
{
#if UNITY_EDITOR
    [CustomEditor(typeof(UIController))]
    public class UIControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            UIController uiController = target as UIController;
            if (uiController != null)
            {
                if (GUILayout.Button("FindUIControllerItems"))
                {
                    uiController.FindUIControllerItems();
                }
            }

            base.OnInspectorGUI();
        }
    }
#endif
}