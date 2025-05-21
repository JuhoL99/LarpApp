using UnityEngine;

public class Save
{
    public string playerName;
    public string playerCards;
    public string linkedUserNames;
    public string linkedUserCards;
    public string linkedUserNotes;
    public string linkedUserNotesNew;

    public Save(string playerName, string playerCards, string linkedUserNames, string linkedUserCards, string linkedUserNotesNew)
    {
        this.playerName = playerName;
        this.playerCards = playerCards;
        this.linkedUserNames = linkedUserNames;
        this.linkedUserCards = linkedUserCards;
        this.linkedUserNotesNew = linkedUserNotesNew;
    }
}
