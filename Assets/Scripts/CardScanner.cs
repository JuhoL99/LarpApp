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
    [Header("Other")]
    [SerializeField] private Transform imageTargetParent;
    [SerializeField] private bool printLogInfo;
    public UnityEvent<bool> onScanToggled;
    public UnityEvent<CardSO> onCardScanned;
    private ImageTrackerFrameFilter imageTracker;
    private CameraDeviceFrameSource cameraDevice;
    private bool isScanning;
    private CardSO currentSelectedCard;

    private void Awake()
    {
        imageTracker = session.GetComponentInChildren<ImageTrackerFrameFilter>();
        cameraDevice = session.GetComponentInChildren<CameraDeviceFrameSource>();
        cardTargetArray = imageTargetParent.GetComponentsInChildren<ImageTargetController>();
        foreach (var cardTarget in cardTargetArray)
        {
            imageTracker.LoadTarget(cardTarget);
            if(printLogInfo) cardTarget.TargetLoad += TargetLoadedCheck;
            AddTargetControllerEvents(cardTarget);
        }
    }
    private void TargetLoadedCheck(easyar.Target trg, bool val)
    {
        Debug.Log($"Target: {trg.name()}, Value: {val}");
    }
    private void Start()
    {
        onCardScanned.AddListener(SetCurrentSelectedCard);
    }
    public void EnableScanning()
    {
        currentSelectedCard = null;
        ToggleTracking(true);
        isScanning = true;
        onScanToggled?.Invoke(true);
    }
    public void DisableScanning()
    {
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
    private void SetCurrentSelectedCard(CardSO card)
    {
        currentSelectedCard = card;
    }
    public CardSO GetCurrentSelectedCard()
    {
        return currentSelectedCard;
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
    private void AssignCardToUserOrPlayer(PlayerData player, int index)
    {
        player.AddCardToPlayer(currentSelectedCard, index);
    }
    private void AssignCardToUserOrPlayer(UserData user, int index)
    {
        user.AddCardToUser(currentSelectedCard, index);
    }
}
