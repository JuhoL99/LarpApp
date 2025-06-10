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
    [SerializeField] private GameObject createEntryPanel;
    [SerializeField] private TMP_InputField titleInputField;
    [SerializeField] private TMP_InputField contentInputField;
    [SerializeField] private Button addEntryButton;
    [SerializeField] private Button saveEntryButton;
    [SerializeField] private Button cancelEntryButton;
    [SerializeField] private TMP_Text panelTitleText; // Add this reference

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
        cancelEntryButton.onClick.AddListener(CancelEntry);
    }

    private IEnumerator DelayStart()
    {
        yield return null;
        LoadEntriesFromFile();
        PopulateEntries();
        diaryContent.ClosePage();
        createEntryPanel.SetActive(false);
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

        // Connect edit and delete functionality for this specific entry
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
        createEntryPanel.SetActive(true);

        // Focus on title field
        titleInputField.Select();
        panelTitleText.text = "New Entry";
    }

    public void OpenEditEntryPanel(DiaryEntry entry, DiaryEntryManager manager)
    {
        isEditingMode = true;
        currentlyEditingEntry = entry;
        currentlyEditingManager = manager;

        titleInputField.text = entry.entryTitle;
        contentInputField.text = entry.entryText;
        createEntryPanel.SetActive(true);

        titleInputField.Select();
        panelTitleText.text = "Edit Entry";
    }

    public void SaveEntry()
    {
        string title = titleInputField.text.Trim();
        string content = contentInputField.text.Trim();

        // Validate input
        if (string.IsNullOrEmpty(title))
        {
            Debug.LogWarning("Title cannot be empty!");
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

            Debug.Log("Entry updated successfully!");
        }
        else
        {
            // Create new entry
            DiaryEntry newEntry = new DiaryEntry(title, content, DateTime.Now);
            AddEntry(newEntry);
            Debug.Log("New entry created successfully!");
        }

        CancelEntry(); // Close the panel
    }

    public void CancelEntry()
    {
        createEntryPanel.SetActive(false);
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

    // Test method - you can remove this later
    public void SaveTest()
    {
        AddEntry(new DiaryEntry("Test Entry", "This is a test entry created programmatically.", DateTime.Now));
    }
}