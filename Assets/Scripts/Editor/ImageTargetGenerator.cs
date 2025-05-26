using easyar;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ImageTargetGenerator : EditorWindow
{
    [MenuItem("Tools/Image Target Generator")]
    public static void ShowWindow()
    {
        GetWindow<ImageTargetGenerator>("Image Target Generator");
    }
    private void OnGUI()
    {
        GUILayout.Label("Generate image targets from StreamingAssets");
        if(GUILayout.Button("Generate"))
        {
            CreateAllTargets();
        }
    }
    private void CreateAllTargets()
    {
        GameObject parent = new GameObject("Image Target Parent");
        DirectoryInfo info = new DirectoryInfo(Application.streamingAssetsPath);
        if (!info.Exists) return;
        var fileInfo = info.GetFiles();
        foreach(var file in fileInfo)
        {
            if(file.Name.Contains(".meta")) continue;
            if(!file.Name.Contains(".jpg") && !file.Name.Contains(".png")) continue;
            GenerateTarget(file.Name, parent);
        }
    }
    private void GenerateTarget(string file, GameObject parent)
    {
        GameObject go = new GameObject();
        go.transform.parent = parent.transform;
        go.name = $"Image Target - {file}";
        ImageTargetController controller = go.AddComponent<ImageTargetController>();
        controller.ImageFileSource.Path = file;
        controller.ImageFileSource.Name = file.Substring(0, file.LastIndexOf('.'));
        controller.ImageFileSource.Scale = 0.1f;
    }
}
