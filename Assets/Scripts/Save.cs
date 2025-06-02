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
    public string linkedUserNotes;
    public string linkedUserNotesNew;

    public Save(string playerName, string playerNotes, string playerCards, string linkedUserNames, string linkedUserCards, string linkedUserNotesNew)
    {
        this.playerName = playerName;
        this.playerNotes = playerNotes;
        this.playerCards = playerCards;
        this.linkedUserNames = linkedUserNames;
        this.linkedUserCards = linkedUserCards;
        this.linkedUserNotesNew = linkedUserNotesNew;
    }
}
