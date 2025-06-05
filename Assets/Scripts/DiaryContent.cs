using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiaryContent : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text contentText;
    [SerializeField] private Button closePanelButton;
    [SerializeField] private GameObject page;

    private void Start()
    {
        GameManager.instance.onBackAction.AddListener(ClosePanel);
    }
    private void OnEnable()
    {
        if(GameManager.instance != null) GameManager.instance.onBackAction.AddListener(ClosePanel);
        closePanelButton.onClick.AddListener(ClosePanel);
    }
    private void OnDisable()
    {
        GameManager.instance.onBackAction.RemoveListener(ClosePanel);
        closePanelButton.onClick.RemoveListener(ClosePanel);
    }
    public void SetupDiaryContent(DiaryEntry entry)
    {
        titleText.text = entry.entryTitle;
        contentText.text = entry.entryText;
        page.SetActive(true);
    }
    private void ClosePanel()
    {
        titleText.text = string.Empty;
        contentText.text = string.Empty;
        page.SetActive(false);
    }
}
