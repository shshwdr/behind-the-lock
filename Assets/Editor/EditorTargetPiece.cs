using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TargetPiece))]
public class EditorTargetPiece : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TargetPiece myScript = (TargetPiece)target;
        if (GUILayout.Button("Align Piece"))
        {
            myScript.Align();
        }
    }
}