using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScannerPanelManager : MonoBehaviour
{
    [Header("Panel Object")]
    [SerializeField] private GameObject mainPanel;
    [Header("Scan Frame")]
    [SerializeField] private GameObject scanPanel;
    [Header("Assigning Menu")]
    [SerializeField] private GameObject assignPanel;
    [Header("Assing Dropdown")]
    [SerializeField] private TMP_Dropdown dropdown; //temp
    private void Start()
    {
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
        mainPanel.SetActive(true);
        scanPanel.SetActive(true);
    }
    private void ScanFinished()
    {
        mainPanel.SetActive(false);
        scanPanel.SetActive(false);
        assignPanel.SetActive(true);
    }
    private void SetUpDropdownMenu()
    {

    }
}
