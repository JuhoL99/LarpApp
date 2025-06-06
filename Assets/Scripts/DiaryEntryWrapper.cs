using System;
using UnityEngine;

[System.Serializable]
public class DiaryEntryWrapper
{
    public string title;
    public string text;
    public string time;
    //public DateTime time; //jsonutility doesnt recognice? turn to string & parse
}
