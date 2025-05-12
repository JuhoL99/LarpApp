using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [Header("Visuals")]
    [SerializeField] private Image cardRenderer;
    [SerializeField] private GameObject cardButton;
    [SerializeField] private GameObject scanningPanel;

    private void Start()
    {
        GameManager.instance.cardScanner.onScanToggled.AddListener(ToggleScanVisuals);
        scanningPanel.SetActive(false);
    }
    private void ToggleScanVisuals(bool val)
    {
        cardRenderer.enabled = !val;
        cardButton.SetActive(!val);
        scanningPanel.SetActive(val);
    }
}
