using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class ScannerPanelManager : MonoBehaviour
{
    [Header("Background")]
    [SerializeField] private GameObject bgPanel;
    [Header("Panel Object")]
    [SerializeField] private GameObject mainPanel;
    [Header("Scan Frame")]
    [SerializeField] private GameObject scanPanel;
    [Header("Selection Menu")]
    [SerializeField] private GameObject selectionPanel;
    [SerializeField] private Image scannedCardPreviewImage;
    [Header("Buttons")]
    [SerializeField] private Button assignToSelfButton;
    [SerializeField] private Button assignToExistingButton;
    [SerializeField] private Button assignToNewButton;
    [Header("Events")]
    public UnityEvent<CardSO> onCardAssignRequestToNewUser;
    public UnityEvent<CardSO> onCardAssignRequestToExisitingUser;
    public UnityEvent<CardSO> onCardAssignRequestToPlayer;
    public UnityEvent enableScanSelectionPanel;
    public UnityEvent onMapCardScanned;
    private CardSO currentScannedCard;

    //disable scanning on back button and action later so no unexpected card switches
    private void Start()
    {
        GameManager.instance.cardScanner.onCardScanned.AddListener(HandleCardScanned);
        scanPanel.SetActive(false);
        selectionPanel.SetActive(false);

        //temp >>
        GameManager.instance.cardScanner.onScanToggled.AddListener(HandleScanCanceled);
    }
    public void HandleCardManualAssignment(CardSO card)
    {
        Debug.Log($"In manual assignent with card: {card}");
        enableScanSelectionPanel?.Invoke();
        HandleCardScanned(card);
    }
    private void HandleCardScanned(CardSO card)
    {
        Debug.Log("Card scanned");
        currentScannedCard = card;
        Debug.Log($"currentscannedcard: {currentScannedCard}");
        ToggleBackground(true);
        scanPanel.SetActive(false);
        MarkerType marker = CheckScannedMarkerType();
        
        switch(marker)
        {
            case(MarkerType.Archetype):
                OpenSelectionPanel(card);
                break;
            case(MarkerType.Map):
                //open map
                break;
            case (MarkerType.Fate):
                //just in case we want to scan later?
                break;
            case(MarkerType.Info):
                //idk
                break;
            default:
                break;
        }
    }
    public void EnableScanPanel()
    {
        ToggleBackground(false);
        selectionPanel.SetActive(false);
        scanPanel.SetActive(true);
    }
    public void DisableScanPanel()
    {
        ToggleBackground(true);
        scanPanel.SetActive(false);
    }
    private void OpenSelectionPanel(CardSO card = null)
    {
        Debug.Log($"opened selection panel with card: {card}");
        if (card != null)
            scannedCardPreviewImage.sprite = card.GetCardVisual()[0];
        else scannedCardPreviewImage.sprite = null;
        selectionPanel.SetActive(true);
        AddButtonListeners();
    }
    private void AddButtonListeners()
    {
        assignToSelfButton.onClick.AddListener(AssignToSelf);
        assignToExistingButton.onClick.AddListener(AssignToExistingUser);
        assignToNewButton.onClick.AddListener(AssignToNewUser);
    }
    private void RemoveButtonListeners()
    {
        assignToSelfButton.onClick.RemoveListener(AssignToSelf);
        assignToExistingButton.onClick.RemoveListener(AssignToExistingUser);
        assignToNewButton.onClick.RemoveListener(AssignToNewUser);
    }
    public void HideSelectionPanel()
    {
        RemoveButtonListeners();
        //currentScannedCard = null;
        selectionPanel.SetActive(false);
    }
    public void AssignToSelf()
    {
        onCardAssignRequestToPlayer?.Invoke(currentScannedCard);
        HideSelectionPanel();
    }
    public void AssignToExistingUser()
    {
        onCardAssignRequestToExisitingUser?.Invoke(currentScannedCard);
        HideSelectionPanel();
    }
    public void AssignToNewUser()
    {
        onCardAssignRequestToNewUser?.Invoke(currentScannedCard);
        HideSelectionPanel();
    }
    private MarkerType CheckScannedMarkerType()
    {
        return MarkerType.Archetype; //for now
    }
    //temporary >>
    private void HandleScanCanceled(bool toggle)
    {
        if(toggle == false) ToggleBackground(true);
    }
    private void ToggleBackground(bool value)
    {
        bgPanel.SetActive(value);
    }
    //<<
}
