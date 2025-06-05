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
    private void Start()
    {
        diaryEntries = GenerateFakeEntries();
        LoadEntriesFromFile();
        PopulateEntries();
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
            //set diarycontent to entry manager here and enable/disable listeners in script l8r
        }
    }
    private void LoadEntriesFromFile()
    {
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
