using UnityEngine;

public class TogglePanel : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject panelObject;
    private bool toggle;

    private void Start()
    {
        if(panelObject != null) toggle = panelObject.activeSelf;
    }
    public void Toggle()
    {
        if (panelObject == null) return;
        toggle = !toggle;
        panelObject.SetActive(toggle);
    }
}
