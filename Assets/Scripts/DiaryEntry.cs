using System;
using UnityEngine;

public class DiaryEntry
{
    public DateTime entryTime;
    public string entryTitle;
    public string entryText;

    public DiaryEntry(string title, string text, DateTime time)
    {
        entryTitle = title;
        entryText = text;
        entryTime = time;
    }
}