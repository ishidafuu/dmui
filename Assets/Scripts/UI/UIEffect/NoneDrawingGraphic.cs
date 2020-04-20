using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class NoneDrawingGraphic : Graphic
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

[CanEditMultipleObjects, CustomEditor(typeof(NoneDrawingGraphic), false)]
public class NoneDrawingGraphicEditor : GraphicEditor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(m_Script, new GUILayoutOption[0]);
        RaycastControlsGUI();
        serializedObject.ApplyModifiedProperties();
    }
}