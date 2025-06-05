using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class DiaryEntryManager : MonoBehaviour
{
    public UnityEvent<DiaryEntry> onEnableContent = new UnityEvent<DiaryEntry>();
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private Button openDiaryEntryButton;
    private DiaryEntry assignedEntry;

    private void OnEnable()
    {
        openDiaryEntryButton.onClick.AddListener(EnableContent);
    }
    private void OnDisable()
    {
        openDiaryEntryButton.onClick.RemoveListener(EnableContent);
    }
    public void UpdateEntry(DiaryEntry diaryEntry)
    {
        assignedEntry = diaryEntry;
        UpdateVisuals();
    }
    private void UpdateVisuals()
    {
        titleText.text = assignedEntry.entryTitle;
        timeText.text = assignedEntry.entryTime.ToString("g");
    }
    public void EnableContent()
    {
        onEnableContent?.Invoke(assignedEntry);
    }
}
