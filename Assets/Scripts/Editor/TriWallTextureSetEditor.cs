using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TriWallTextureSet))]

public class TriWallTextureSetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TriWallTextureSet triCode = (TriWallTextureSet)target;

        GUILayout.Label("---Editor Buttons---");

        if (GUILayout.Button("Update"))
        {
            triCode.SetupTextures();
        }
    }
}
