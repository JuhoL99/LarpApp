using easyar;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

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
    private InputAction backAction = new InputAction("Back", InputActionType.Button, "<Keyboard>/escape");
    //Add cancel scanning mode on back button

    private void Awake()
    {
        imageTracker = session.GetComponentInChildren<ImageTrackerFrameFilter>();
        cameraDevice = session.GetComponentInChildren<CameraDeviceFrameSource>();
        cardTargetArray = imageTargetParent.GetComponentsInChildren<ImageTargetController>();
        foreach (var cardTarget in cardTargetArray)
        {
            imageTracker.LoadTarget(cardTarget);
            cardTarget.Tracker = imageTracker;
            if(printLogInfo) cardTarget.TargetLoad += TargetLoadedCheck;
            AddTargetControllerEvents(cardTarget);
        }
    }
    private void Start()
    {
        onCardScanned.AddListener(SetCurrentSelectedCard);
        ToggleTracking(false);
        backAction.Enable();
        backAction.performed += HandleInput;
    }
    //need to load targets before they can be detected properly
    private void TargetLoadedCheck(easyar.Target trg, bool val)
    {
        Debug.Log($"Target: {trg.name()}, Value: {val}");
    }
    private void HandleInput(InputAction.CallbackContext ctx)
    {
        if(ctx.performed && isScanning == true) DisableScanning();
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
        cameraDevice.enabled = val;
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
