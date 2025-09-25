using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BakeSmoothNormalsToUV3
{
    [MenuItem("Assets/Bake Smooth Normals to UV3")]
    static void Bake()
    {
        Mesh mesh = Selection.activeObject as Mesh;
        if (mesh == null)
        {
            Debug.LogError("Select a mesh asset (not a model file) in the Project window.");
            return;
        }

        Mesh meshCopy = Object.Instantiate(mesh);
        meshCopy.name = mesh.name + "_SmoothNormals";

        meshCopy.RecalculateNormals();
        var normals = meshCopy.normals;
        var normalsList = new List<Vector3>(normals);

        meshCopy.SetUVs(2, normalsList);

        string path = AssetDatabase.GetAssetPath(mesh);
        string newPath = path.Replace(".asset", "_SmoothNormals.asset");
        AssetDatabase.CreateAsset(meshCopy, newPath);
        AssetDatabase.SaveAssets();

        Debug.Log("Smooth normals baked to UV3 and saved as: " + newPath);
    }
}