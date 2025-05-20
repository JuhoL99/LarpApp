using easyar;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceFileType : MonoBehaviour
{
    [SerializeField] private string[] fileType = new string[] { ".png", ".etd" };
    [SerializeField] private int fileTypeIndex = 1;
    private List<ImageTargetController> imageTargets;

    private void Start()
    {
        imageTargets = new List<ImageTargetController>();
        foreach(Transform t in GetComponentInChildren<Transform>())
        {
            imageTargets.Add(t.gameObject.GetComponent<ImageTargetController>());
        }
        foreach(ImageTargetController t in imageTargets)
        {
            string full = t.ImageFileSource.Path;
            string[] parts = full.Split('.');
            string name = parts[0] + fileType[fileTypeIndex];
            t.ImageFileSource.Path = name;
        }
    }
}
