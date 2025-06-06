using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using easyar;

public class DiaryPanelManager : MonoBehaviour
{
    private List<DiaryEntry> diaryEntries;
    private DiaryEntryManager[] diaryEntryObjects;
    [SerializeField] private DiaryContent diaryContent;
    [SerializeField] private GameObject diaryEntryPrefab;
    [SerializeField] private Transform scrollContentObject;

    //add button with input field to create new diary entry objects
    //add inputfield and check active diaryentry change text based on field/load text into field
    private void Start()
    {
        //diaryEntries = GenerateFakeEntries();
        StartCoroutine(DelayStart());
    }
    private IEnumerator DelayStart()
    {
        yield return null;
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
        Debug.Log(diaryEntries.Count);
        for(int i = 0; i < diaryEntries.Count; i++)
        {
            GameObject go = Instantiate(diaryEntryPrefab,scrollContentObject);
            DiaryEntryManager mgr = go.GetComponent<DiaryEntryManager>();
            mgr.UpdateEntry(diaryEntries[i]);
            mgr.onEnableContent.AddListener(diaryContent.SetupDiaryContent);
            //set diarycontent to entry manager here and enable/disable listeners in script l8r to reduce simultaneous listeners
        }
    }
    private void LoadEntriesFromFile()
    {
        diaryEntries = GameManager.instance.player.diaryEntries;
        return;
    }
    public void AddEntry(DiaryEntry entry)
    {
        //diaryEntries.Add(entry); //if you copy player.diaryEntries instead of referencing to it
        GameObject go = Instantiate(diaryEntryPrefab, scrollContentObject);
        DiaryEntryManager mgr = go.GetComponent<DiaryEntryManager>();
        mgr.UpdateEntry(entry);
        mgr.onEnableContent.AddListener(diaryContent.SetupDiaryContent);
        GameManager.instance.player.AddDiaryEntry(entry);
        
    }
    public void SaveTest()
    {
        AddEntry(new DiaryEntry("title", "text", DateTime.Now));
    }
}
