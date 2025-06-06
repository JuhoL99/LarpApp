using UnityEngine;
using System;
using System.Collections.Generic;

public class DiaryPanelManager : MonoBehaviour
{
    private List<DiaryEntry> diaryEntries;
    private DiaryEntryManager[] diaryEntryObjects;
    [SerializeField] private DiaryContent diaryContent;
    [SerializeField] private GameObject diaryEntryPrefab;
    [SerializeField] private Transform scrollContentObject;

    //add button with input field to create new diary entry objects
    //add similar inputfield and saving as in profile and connections to write into diary entry
    private void Start()
    {
        diaryEntries = GenerateFakeEntries();
        LoadEntriesFromFile();
        PopulateEntries();
        diaryContent.ClosePage();
    }
    private List<DiaryEntry> GenerateFakeEntries()
    {
        List<DiaryEntry> entries = new List<DiaryEntry>();
        for(int i = 0; i < 15; i++)
        {
            DiaryEntry entry = new DiaryEntry($"entry no.{i}", "writing", DateTime.Now);
            entries.Add(entry);
        }
        return entries;
    }
    private void PopulateEntries()
    {
        for(int i = 0; i < diaryEntries.Count; i++)
        {
            GameObject go = Instantiate(diaryEntryPrefab,scrollContentObject);
            DiaryEntryManager mgr = go.GetComponent<DiaryEntryManager>();
            mgr.UpdateEntry(diaryEntries[i]);
            mgr.onEnableContent.AddListener(diaryContent.SetupDiaryContent);
            //set diarycontent to entry manager here and enable/disable listeners in script l8r to reduce simultaneous listeners
        }
    }
    private void SaveDiaryEntries()
    {
        //add stuff 
    }
    private void LoadEntriesFromFile()
    {
        if(GameManager.instance != null) diaryEntries = GameManager.instance.player.diaryEntries;
        return;
    }
    public void AddEntry(DiaryEntry entry)
    {
        diaryEntries.Add(entry);
        GameObject go = Instantiate(diaryEntryPrefab, scrollContentObject);
        DiaryEntryManager mgr = go.GetComponent<DiaryEntryManager>();
        mgr.UpdateEntry(entry);
        mgr.onEnableContent.AddListener(diaryContent.SetupDiaryContent);
    }
}
