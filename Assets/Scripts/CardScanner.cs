using easyar;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CardScanner : MonoBehaviour
{
    [Header("AR Session gameobject")]
    [SerializeField] private ARSession session;
    [Header("Image target testing")]
    [SerializeField] private ImageTargetController[] cardTargetArray;
    [SerializeField] private List<ImageTargetController> cardTargetList = new List<ImageTargetController>();
    [Header("Debug texts")]
    [SerializeField] private TMP_Text targetInfo;
    [Header("Other")]
    [SerializeField] private TextAsset text; //test
    [SerializeField] private Transform imageTargetParent;
    public UnityEvent<bool> onScanToggled;
    public UnityEvent<CardSO> onCardScanned;
    private ImageTrackerFrameFilter imageTracker;
    private CameraDeviceFrameSource cameraDevice;
    private bool isScanning;
    [SerializeField] private bool generateImageTargets = false; //turn to prefab, unpack, delete from folder

    private void Awake()
    {
        imageTracker = session.GetComponentInChildren<ImageTrackerFrameFilter>();
        cameraDevice = session.GetComponentInChildren<CameraDeviceFrameSource>();
        cardTargetArray = imageTargetParent.GetComponentsInChildren<ImageTargetController>();
        foreach (var card in cardTargetArray)
        {
            AddTargetControllerEvents(card);
        }
    }
    private void Start()
    {
        if (!generateImageTargets) return;
        DirectoryInfo info;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        info = new DirectoryInfo(Application.streamingAssetsPath);
#elif UNITY_ANDROID
        info = new DirectoryInfo($"jar:file://{Application.streamingAssetsPath}"); //doesnt work need to use unity webrequest
#endif
        if (info == null) return;
        var fileInfo = info.GetFiles();
        foreach(var file in fileInfo)
        {
            if (file.Name.Contains(".meta")) continue;
            //if (!file.Name.Contains(".jpg") || !file.Name.Contains(".png")) continue;
            GenerateImageTargetsFromFolder(file.Name); //only works in editor
        }
        
    }
    private void GenerateImageTargetsFromFolder(string cardName) 
    {
        GameObject go = new GameObject();
        go.transform.parent = transform;
        go.name = $"Image Target - {cardName}";
        ImageTargetController controller = go.AddComponent<ImageTargetController>();
        controller.Tracker = imageTracker;
        controller.ImageFileSource.Path = cardName;
        controller.ImageFileSource.Name = cardName.Substring(0,cardName.LastIndexOf('.'));
        controller.ImageFileSource.Scale = 0.1f;
        cardTargetList.Add(controller);
        AddTargetControllerEvents(controller);
    }
    public void EnableScanning()
    {
        //test
        foreach (var card in cardTargetArray)
        {
            imageTracker.LoadTarget(card);
        }
        //test
        ToggleTracking(true);
        isScanning = true;
        onScanToggled?.Invoke(true);
    }
    public void DisableScanning()
    {
        //test
        foreach (var card in cardTargetArray)
        {
            imageTracker.UnloadTarget(card);
        }
        //test
        ToggleTracking(false);
        isScanning = false;
        onScanToggled?.Invoke(false);
    }
    public void ToggleTracking(bool val)
    {
        imageTracker.enabled = val;
    }
    private void AddTargetControllerEvents(ImageTargetController controller)
    {
        controller.TargetFound += () => TargetFound(controller.ImageFileSource.Name);
    }
    private void TargetFound(string targetID)
    {
        if (!isScanning) return;
        else
        {
            onCardScanned?.Invoke(GameManager.instance.cardDatabase.GetCardByName(targetID));
            DisableScanning();
        }
    }
}
