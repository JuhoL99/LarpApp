using UnityEngine;

public class OnSettingsEnabled : MonoBehaviour
{
    [SerializeField] private GameObject warningPanel;
    private void OnEnable()
    {
        warningPanel.SetActive(false);
    }
}
