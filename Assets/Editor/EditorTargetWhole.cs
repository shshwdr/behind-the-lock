using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LevelTargetEditorHelper))]
public class EditorTargetWhole : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelTargetEditorHelper myScript = (LevelTargetEditorHelper)target;
        if (GUILayout.Button("Align Collider"))
        {
            myScript.Align();
        }
    }
}