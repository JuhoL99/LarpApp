using easyar;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CardScanner : MonoBehaviour
{
    [Header("AR Session gameobject")]
    [SerializeField] private ARSession session;
    [Header("Image target testing")]
    [SerializeField] private ImageTargetController cardTarget;
    [Header("Debug texts")]
    [SerializeField] private TMP_Text targetInfo;
    [Header("Other")]
    public UnityEvent<CardSO> onCardScanned;
    private ImageTrackerFrameFilter imageTracker;
    private CameraDeviceFrameSource cameraDevice;
    private bool isScanning;

    private void Awake()
    {
        imageTracker = session.GetComponentInChildren<ImageTrackerFrameFilter>();
        cameraDevice = session.GetComponentInChildren<CameraDeviceFrameSource>();
        AddTargetControllerEvents(cardTarget);
    }
    public void EnableScanning()
    {
        isScanning = true;
    }
    public void DisableScanning()
    {
        isScanning = false;
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
