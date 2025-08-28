using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TriWallGridSetup))]

public class TriWallGridSetupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TriWallGridSetup triCode = (TriWallGridSetup)target;

        GUILayout.Label("---Editor Buttons---");

        if (GUILayout.Button("Update"))
        {
            triCode.SetAlpha();
        }
    }
}
