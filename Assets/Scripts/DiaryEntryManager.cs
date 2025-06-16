using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DiaryEntryManager : MonoBehaviour
{
    public UnityEvent<DiaryEntry> onEnableContent = new UnityEvent<DiaryEntry>();
    public UnityEvent<DiaryEntry, DiaryEntryManager> onEditEntry = new UnityEvent<DiaryEntry, DiaryEntryManager>();
    public UnityEvent<DiaryEntry> onDeleteEntry = new UnityEvent<DiaryEntry>();

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

    public void EditEntry()
    {
        onEditEntry?.Invoke(assignedEntry, this);
    }

    public void DeleteEntry()
    {
        if (ShowDeleteConfirmation())
        {
            onDeleteEntry?.Invoke(assignedEntry);
        }
    }

    private bool ShowDeleteConfirmation()
    {
        // Placeholder if confirmation is implemented in the future
        return true;
    }

    public void RefreshVisuals()
    {
        if (assignedEntry != null)
        {
            UpdateVisuals();
        }
    }
}