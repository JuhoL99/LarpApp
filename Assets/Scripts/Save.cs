using UnityEngine;

public class Save
{
    public string playerName;
    public string playerNotes;
    public string playerCards;
    public string playerFateCards;
    public string linkedUserNames;
    public string linkedUserCards;
    public string linkedUserFateCards;
    public string linkedUserNotesNew;
    public string diaryEntries;

    public Save(string playerName, string playerNotes, string playerCards,string playerFateCards, string linkedUserNames, string linkedUserCards,string linkedUserFateCards, string linkedUserNotesNew)
    {
        this.playerName = playerName;
        this.playerNotes = playerNotes;
        this.playerCards = playerCards;
        this.playerFateCards = playerFateCards;
        this.linkedUserNames = linkedUserNames;
        this.linkedUserCards = linkedUserCards;
        this.linkedUserFateCards = linkedUserFateCards;
        this.linkedUserNotesNew = linkedUserNotesNew;
    }
}
