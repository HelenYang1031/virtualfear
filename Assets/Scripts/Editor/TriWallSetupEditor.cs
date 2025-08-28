using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TriWallSetup))]
public class TriWallSetupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TriWallSetup triCode = (TriWallSetup)target;

        GUILayout.Label("---Editor Buttons---");

        if (GUILayout.Button("Update"))
        {
            triCode.SetMats();
            triCode.SetWalls();
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("HideAll"))
        {
            triCode.blueVisible = false;
            triCode.greenVisible = false;
            triCode.redVisible = false;
            triCode.SetWalls();
        }
        if (GUILayout.Button("UnhideAll"))
        {
            triCode.blueVisible = true;
            triCode.greenVisible = true;
            triCode.redVisible = true;
            triCode.SetWalls();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("-"))
        {
            triCode.blueVisible = true;
            triCode.greenVisible = false;
            triCode.redVisible = false;
            triCode.SetWalls();
        }
        if (GUILayout.Button("\\"))
        {
            triCode.blueVisible = false;
            triCode.greenVisible = true;
            triCode.redVisible = false;
            triCode.SetWalls();
        }
        if (GUILayout.Button("/"))
        {
            triCode.blueVisible = false;
            triCode.greenVisible = false;
            triCode.redVisible = true;
            triCode.SetWalls();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("/\\"))
        {
            triCode.blueVisible = false;
            triCode.greenVisible = true;
            triCode.redVisible = true;
            triCode.SetWalls();
        }
        if (GUILayout.Button("/-"))
        {
            triCode.blueVisible = true;
            triCode.greenVisible = false;
            triCode.redVisible = true;
            triCode.SetWalls();
        }
        if (GUILayout.Button("\\-"))
        {
            triCode.blueVisible = true;
            triCode.greenVisible = true;
            triCode.redVisible = false;
            triCode.SetWalls();
        }
        GUILayout.EndHorizontal();
    }
}
