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
        if(playerArchetypeCards != null && index < maxCardAmount)
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
        return s.Length > 0 ? s.Substring(0, s.Length - 1) : string.Empty;
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
        return s.Length > 0 ? s.Substring(0, s.Length -1) : string.Empty;
    }
    public string GetUserNotesString()
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
    public string GetUserRelationsString()
    {
        UserRelationsWrapper userRelations = new UserRelationsWrapper();
        userRelations.users = new List<UserIdPair>();
        foreach (UserData user in playerAddedRelations)
        {
            userRelations.users.Add(new UserIdPair(user.userName,user.userID));
        }
        string data = JsonUtility.ToJson(userRelations);
        return data;
    }
    public void LoadPlayerCardsFromString(string loadData)
    {
        if (string.IsNullOrEmpty(loadData)) return;
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
        if(string.IsNullOrEmpty(loadData)) return;
        UserRelationsWrapper userData = JsonUtility.FromJson<UserRelationsWrapper>(loadData);
        List<UserIdPair> data = userData.users;
        foreach(UserIdPair item in data)
        {
            UserData user = new UserData(item.userName,item.userID);
            AddUserToRelations(user);
        }
    }
    public void LoadUserCardsFromString(string loadData)
    {
        if (string.IsNullOrEmpty(loadData)) return;
        string s = string.Empty;
        string[] data = loadData.Split(",");
        int i = 0;
        foreach(UserData user in playerAddedRelations)
        {
            string[] parts = data[i].Split("|");
            //Debug.Log(parts[1] + " " + parts[0]);
            int j = 0;
            foreach(string part in parts)
            {
                user.userArchetypeCards[j] = GameManager.instance.cardDatabase.GetCardByID(int.Parse(part));
                j++;
            }
            i++;
        }
    }
    public void LoadUserNotesFromString(string loadData)
    {
        if (string.IsNullOrEmpty(loadData)) return;
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
