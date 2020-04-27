using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class NoneGraphicRaycaster : Graphic
{
    public override void SetMaterialDirty()
    { }

    public override void SetVerticesDirty()
    { }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
    }
}

[CanEditMultipleObjects, CustomEditor(typeof(NoneGraphicRaycaster), false)]
public class NoneGraphicRaycasterEditor : GraphicEditor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(m_Script, new GUILayoutOption[0]);
        RaycastControlsGUI();
        serializedObject.ApplyModifiedProperties();
    }
}