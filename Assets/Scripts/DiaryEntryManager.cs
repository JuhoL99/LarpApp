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
    [SerializeField] private Button deleteButton; // Optional: dedicated delete button

    private DiaryEntry assignedEntry;

    private void OnEnable()
    {
        openDiaryEntryButton.onClick.AddListener(EnableContent);

        if (deleteButton != null)
            deleteButton.onClick.AddListener(DeleteEntry);
    }

    private void OnDisable()
    {
        openDiaryEntryButton.onClick.RemoveListener(EnableContent);

        if (deleteButton != null)
            deleteButton.onClick.RemoveListener(DeleteEntry);
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
        // You might want to add a confirmation dialog here
        if (ShowDeleteConfirmation())
        {
            onDeleteEntry?.Invoke(assignedEntry);
        }
    }

    private bool ShowDeleteConfirmation()
    {
        // For now, just return true. You could implement a proper confirmation dialog
        // or use Unity's EditorUtility.DisplayDialog for testing
        return true;
    }
}