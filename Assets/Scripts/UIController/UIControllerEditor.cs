using UnityEditor;
using UnityEngine;

namespace DM
{
    [CustomEditor(typeof(UIController))]
    public class UIControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            UIController uiController = target as UIController;
            if (uiController != null)
            {
                if (GUILayout.Button("FindRayCaster"))
                {
                    uiController.FindUIControllerItems();
                }
            }

            base.OnInspectorGUI();
        }
    }
}