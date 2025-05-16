using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerData
{
    public string playerName;
    public CardSO[] playerArchetypeCards;
    public List<UserData> playerAddedRelations;
    public int maxCardAmount = 2;

    public PlayerData(string name = null, CardSO[] cards = null, List<UserData> addedRelations = null)
    {
        playerName = name;
        playerArchetypeCards = cards == null ? new CardSO[maxCardAmount] : cards;
        playerAddedRelations = addedRelations == null ? new List<UserData>() : addedRelations;
    }
    public void ChangePlayerName(string newName)
    {
        if(playerName != newName)
        {
            playerName = newName;
        }
    }
    public void AddUserToRelations(UserData userToAdd)
    {
        if (playerAddedRelations != null)
        {
            playerAddedRelations.Add(userToAdd);
        }
    }
    public void RemoveUserFromRelations(UserData userToRemove)
    {
        if(playerAddedRelations != null && playerAddedRelations.Contains(userToRemove))
        {
            playerAddedRelations.Remove(userToRemove);
        }
    }
    public void AddCardToPlayer(CardSO card, int index)
    {
        if(playerArchetypeCards != null && index <= maxCardAmount)
        {
            playerArchetypeCards[index] = card;
        }
    }
    public string GetPlayerCardString()
    {
        string s = string.Empty;
        for (int i = 0; i < maxCardAmount; i++)
        {
            if (playerArchetypeCards[i] == null) s += "-1|";
            else s += $"{playerArchetypeCards[i].cardId}|";
        }
        return s = s.Substring(0, s.Length - 1);
    }
    public string GetUserCardString() //format 10|23,-1|9,8|54 ...
    {
        string s = string.Empty;
        foreach(UserData user in playerAddedRelations)
        {
            for(int i = 0; i < user.maxCardAmount; i++)
            {
                if (user.userArchetypeCards[i] == null) s += "-1|";
                else s += $"{user.userArchetypeCards[i].cardId}|";
            }
            s = s.Substring(0, s.Length - 1);
            s += ",";
        }
        return s = s.Substring(0, s.Length -1);
    }
    public string GetPlayerNotesString()
    {
        AllNoteWrapper allNotes = new AllNoteWrapper();
        allNotes.allNotesList = new List<NoteWrapper>();
        foreach (UserData user in playerAddedRelations)
        {
            NoteWrapper note = new NoteWrapper();
            note.noteList = user.notesAboutUser;
            allNotes.allNotesList.Add(note);
        }
        string data = JsonUtility.ToJson(allNotes);
        return data;
    }
    public string GetUserRelationsString() //format guid:name,guid:name,guid:name ...
    {
        string s = string.Empty;

        foreach(UserData user in playerAddedRelations)
        {
            s += $"{user.userID}:{user.userName},";
        }
        return s = s.Substring(0, s.Length-1);
    }
    public void LoadPlayerCardsFromString(string loadData)
    {
        string s = string.Empty;
        string[] data = loadData.Split("|");
        int i = 0;
        foreach (string element in data)
        {
            playerArchetypeCards[i] = GameManager.instance.cardDatabase.GetCardByID(int.Parse(element));
            i++;
        }
    }
    public void LoadUsersFromString(string loadData)
    {
        List<UserData> users = new List<UserData>();
        string[] data = loadData.Split(",");
        foreach (var element in data)
        {
            string[] parts = element.Split(":");
            UserData user = new UserData(parts[1], Guid.Parse(parts[0]));
            this.AddUserToRelations(user);
        }
    }
    public void LoadUserCardsFromString(string loadData)
    {
        string s = string.Empty;
        string[] data = loadData.Split(",");
        int i = 0;
        foreach(UserData user in playerAddedRelations)
        {
            string[] parts = data[i].Split("|");
            int j = 0;
            foreach(string part in parts)
            {
                user.userArchetypeCards[i] = GameManager.instance.cardDatabase.GetCardByID(int.Parse(part));
                j++;
            }
            i++;
        }
    }
    public void LoadUserNotesFromString(string loadData)
    {
        AllNoteWrapper data = JsonUtility.FromJson<AllNoteWrapper>(loadData);
        int i = 0;
        foreach(NoteWrapper notes in data.allNotesList)
        {
            foreach(string note in notes.noteList)
            {
                playerAddedRelations[i].AddNoteToUser(note);
            }
            i++;
        }
    }
}
