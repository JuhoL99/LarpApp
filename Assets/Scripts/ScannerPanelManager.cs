using TMPro;
using System.Collections.Generic;
using UnityEngine;

public class ScannerPanelManager : MonoBehaviour
{
    [Header("Panel Object")]
    [SerializeField] private GameObject mainPanel;
    [Header("Scan Frame")]
    [SerializeField] private GameObject scanPanel;
    [Header("Assigning Menu")]
    [SerializeField] private GameObject assignPanel;
    [Header("Assign Dropdown")]
    [SerializeField] private TMP_Dropdown dropdown;
    private void Start()
    {
        dropdown.onValueChanged.AddListener(DropdownSelection);
        mainPanel.SetActive(false);
        scanPanel.SetActive(false);
        assignPanel.SetActive(false);
        GameManager.instance.cardScanner.onScanToggled.AddListener(HandleScanModeChange);
    }
    private void HandleScanModeChange(bool val)
    {
        if(val) ScanStarted();
        else ScanFinished();
    }
    private void ScanStarted()
    {
        SetUpDropdownMenu();
        //
        //assignPanel.SetActive(true);
        //
        mainPanel.SetActive(true);
        scanPanel.SetActive(true);
    }
    private void ScanFinished()
    {
        mainPanel.SetActive(false);
        scanPanel.SetActive(false);
        //assignPanel.SetActive(true);
    }
    // > select card from button > card scan result from gamemanager
    //remove >
    private void SetUpDropdownMenu()
    {
        dropdown.options.Clear();
        List<UserData> addedUsers = GameManager.instance.player.playerAddedRelations;
        List<string> test = new List<string>();
        foreach (UserData user in addedUsers)
        {
            test.Add(user.userName);
        }
        dropdown.AddOptions(test);
    }
    private void DropdownSelection(int value)
    {
        Debug.Log($"value from event: {value}, dropdown value: {dropdown.value}");
        Debug.Log(GameManager.instance.player.playerAddedRelations[value].userName);
        ScanFinished();
    }
}
