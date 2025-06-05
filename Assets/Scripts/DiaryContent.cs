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
        GameManager.instance.onBackAction.AddListener(ClosePage);
    }
    private void OnEnable()
    {
        if(GameManager.instance != null) GameManager.instance.onBackAction.AddListener(ClosePage);
        closePanelButton.onClick.AddListener(ClosePage);
    }
    private void OnDisable()
    {
        GameManager.instance.onBackAction.RemoveListener(ClosePage);
        closePanelButton.onClick.RemoveListener(ClosePage);
    }
    public void SetupDiaryContent(DiaryEntry entry)
    {
        titleText.text = entry.entryTitle;
        contentText.text = entry.entryText;
        page.SetActive(true);
    }
    public void ClosePage()
    {
        titleText.text = string.Empty;
        contentText.text = string.Empty;
        page.SetActive(false);
    }
}
