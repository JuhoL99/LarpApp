using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class DiaryPanelManager : MonoBehaviour
{
    private List<DiaryEntry> diaryEntries;
    private DiaryEntryManager[] diaryEntryObjects;

    [Header("Display Components")]
    [SerializeField] private DiaryContent diaryContent;
    [SerializeField] private GameObject diaryEntryPrefab;
    [SerializeField] private Transform scrollContentObject;

    [Header("Entry Creation UI")]
    [SerializeField] private GameObject entryPanel;
    [SerializeField] private TMP_InputField titleInputField;
    [SerializeField] private TMP_InputField contentInputField;
    [SerializeField] private Button addEntryButton;
    [SerializeField] private Button saveEntryButton;
    [SerializeField] private Button cancelEntryButton;
    [SerializeField] private Button deleteEntryButton;
    [SerializeField] private TMP_Text panelTitleText;

    [Header("Currently Editing")]
    private DiaryEntry currentlyEditingEntry;
    private DiaryEntryManager currentlyEditingManager;
    private bool isEditingMode = false;

    private void Start()
    {
        StartCoroutine(DelayStart());
        SetupButtons();
    }

    private void SetupButtons()
    {
        addEntryButton.onClick.AddListener(OpenCreateEntryPanel);
        saveEntryButton.onClick.AddListener(SaveEntry);
        deleteEntryButton.onClick.AddListener(DeleteCurrentEntry);
        cancelEntryButton.onClick.AddListener(CancelEntry);
    }

    private IEnumerator DelayStart()
    {
        yield return null;
        LoadEntriesFromFile();
        PopulateEntries();
        diaryContent.ClosePage();
        entryPanel.SetActive(false);
    }

    private void PopulateEntries()
    {
        Debug.Log(diaryEntries.Count);

        // Clear existing entries first
        foreach (Transform child in scrollContentObject)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < diaryEntries.Count; i++)
        {
            CreateEntryUI(diaryEntries[i]);
        }
    }

    private void CreateEntryUI(DiaryEntry entry)
    {
        GameObject go = Instantiate(diaryEntryPrefab, scrollContentObject);
        DiaryEntryManager mgr = go.GetComponent<DiaryEntryManager>();
        mgr.UpdateEntry(entry);
        mgr.onEnableContent.AddListener(diaryContent.SetupDiaryContent);

        // Enable edit and delete functionality for this specific entry
        mgr.onEditEntry.AddListener(OpenEditEntryPanel);
        mgr.onDeleteEntry.AddListener(DeleteEntry);
    }

    private void LoadEntriesFromFile()
    {
        diaryEntries = GameManager.instance.player.diaryEntries;
    }

    public void OpenCreateEntryPanel()
    {
        isEditingMode = false;
        currentlyEditingEntry = null;
        currentlyEditingManager = null;

        titleInputField.text = "";
        contentInputField.text = "";
        entryPanel.SetActive(true);

        // Focus on title field
        titleInputField.Select();
        panelTitleText.text = "New Entry";

        // Hide delete button when creating new entry
        deleteEntryButton.gameObject.SetActive(false);
    }

    public void OpenEditEntryPanel(DiaryEntry entry, DiaryEntryManager manager)
    {
        isEditingMode = true;
        currentlyEditingEntry = entry;
        currentlyEditingManager = manager;

        titleInputField.text = entry.entryTitle;
        contentInputField.text = entry.entryText;
        entryPanel.SetActive(true);

        titleInputField.Select();
        panelTitleText.text = "Edit Entry";

        // Show delete button when editing existing entry
        deleteEntryButton.gameObject.SetActive(true);
    }

    public void SaveEntry()
    {
        string title = titleInputField.text.Trim();
        string content = contentInputField.text.Trim();

        // Validate input
        if (string.IsNullOrEmpty(title))
        {
            return;
        }

        if (isEditingMode && currentlyEditingEntry != null)
        {
            // Update existing entry
            currentlyEditingEntry.entryTitle = title;
            currentlyEditingEntry.entryText = content;
            currentlyEditingEntry.entryTime = DateTime.Now; // Update timestamp

            // Update the UI
            currentlyEditingManager.UpdateEntry(currentlyEditingEntry);
        }
        else
        {
            // Create new entry
            DiaryEntry newEntry = new DiaryEntry(title, content, DateTime.Now);
            AddEntry(newEntry);
        }

        CancelEntry(); // Close the panel
    }

    public void DeleteCurrentEntry()
    {
        if (isEditingMode && currentlyEditingEntry != null)
        {
            DeleteEntry(currentlyEditingEntry);
            CancelEntry(); // Close the panel after deletion
        }
    }

    public void CancelEntry()
    {
        entryPanel.SetActive(false);
        isEditingMode = false;
        currentlyEditingEntry = null;
        currentlyEditingManager = null;
    }

    public void AddEntry(DiaryEntry entry)
    {
        // Add to player's diary entries
        GameManager.instance.player.AddDiaryEntry(entry);

        // Create UI element for the new entry
        CreateEntryUI(entry);
    }

    public void DeleteEntry(DiaryEntry entry)
    {
        // Remove from player's diary entries
        GameManager.instance.player.diaryEntries.Remove(entry);

        // Refresh the UI
        PopulateEntries();
    }
}