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
    [Header("Buttons")]
    [SerializeField] private Button assignToSelfButton;
    [SerializeField] private Button assignToExistingButton;
    [SerializeField] private Button assignToNewButton;
    [Header("Events")]
    public UnityEvent<CardSO> onCardAssignRequestToNewUser;
    public UnityEvent<CardSO> onCardAssignRequestToExisitingUser;
    public UnityEvent<CardSO> onCardAssignRequestToPlayer;
    public UnityEvent onMapCardScanned;
    private CardSO currentScannedCard;

    private void Start()
    {
        GameManager.instance.cardScanner.onCardScanned.AddListener(HandleCardScanned);
        scanPanel.SetActive(false);
        selectionPanel.SetActive(false);
    }
    private void HandleCardScanned(CardSO card)
    {
        Debug.Log("Card scanned");
        currentScannedCard = card;
        scanPanel.SetActive(false);
        MarkerType marker = CheckScannedMarkerType();
        
        switch(marker)
        {
            case(MarkerType.Archetype):
                OpenSelectionPanel();
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
        scanPanel.SetActive(true);
        //bgPanel.SetActive(false);
        //Debug.Log("bg set false");
    }
    private void OpenSelectionPanel()
    {
        //bgPanel.SetActive(true);
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
    private void HideSelectionPanel()
    {
        RemoveButtonListeners();
        currentScannedCard = null;
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
}
