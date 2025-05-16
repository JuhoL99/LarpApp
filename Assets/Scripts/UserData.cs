using UnityEngine;
using System.Collections.Generic;
using System;

public class UserData
{
    public Guid userID;
    public string userName;
    public CardSO[] userArchetypeCards;
    public List<string> notesAboutUser;
    public int maxCardAmount = 2;

    public UserData(string name, Guid? id = null, CardSO[] cards = null, List<string> notes = null)
    {
        userID = id ?? Guid.NewGuid();
        userName = name;
        userArchetypeCards = cards == null ? new CardSO[maxCardAmount] : cards;
        notesAboutUser = notes == null ? new List<string>() : notes;
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
        if (userArchetypeCards != null && index <= maxCardAmount)
        {
            userArchetypeCards[index] = card;
        }
    }
    public void AddNoteToUser(string note)
    {
        if(notesAboutUser != null)
        {
            notesAboutUser.Add(note);
        }
    }


}
