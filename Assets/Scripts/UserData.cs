using UnityEngine;
using System.Collections.Generic;
using System;

public class UserData
{
    public Guid userID;
    public string userName;
    public CardSO[] userArchetypeCards;
    public CardSO[] userFateCards;
    public string userNotes;
    public int maxCardAmount = 2;
    public int maxFateCardAmount = 1;

    public UserData(string name, Guid? id = null, CardSO[] cards = null, string notes = null, CardSO[] fateCards = null)
    {
        userID = id ?? Guid.NewGuid();
        userName = name;
        userArchetypeCards = cards == null ? new CardSO[maxCardAmount] : cards;
        userFateCards = fateCards == null ? new CardSO[maxFateCardAmount] : fateCards;
        userNotes = notes == null ? string.Empty : notes;
    }
    public void ChangeUserName(string newName)
    {
        if(userName != newName)
        {
            userName = newName;
        }
    }
    public void AddCardToUser(CardSO card, int index)
    {
        if (userArchetypeCards != null && index < maxCardAmount && card.markerType == MarkerType.Archetype)
        {
            userArchetypeCards[index] = card;
        }
        else if(userFateCards != null && index < maxFateCardAmount && card.markerType == MarkerType.Fate)
        {
            userFateCards[index] = card;
        }
    }
    public void AddUserNotes(string notes)
    {
        userNotes = notes;
    }
}
