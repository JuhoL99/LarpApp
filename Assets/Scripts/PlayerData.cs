using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerData
{
    public string playerName;
    public string playerNotes;
    public CardSO[] playerArchetypeCards;
    public CardSO[] playerFateCards;
    public List<DiaryEntry> diaryEntries;
    public List<UserData> playerAddedRelations;
    public int maxArchetypeCardAmount = 2;
    public int maxFateCardAmount = 3;

    public PlayerData(string name = null, string notes = null, CardSO[] cards = null, CardSO[] fateCards = null, List<UserData> addedRelations = null)
    {
        playerName = name;
        playerNotes = notes;
        playerArchetypeCards = cards == null ? new CardSO[maxArchetypeCardAmount] : cards;
        playerFateCards = cards == null ? new CardSO[maxFateCardAmount] : fateCards;
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
        if(playerArchetypeCards != null && index < maxArchetypeCardAmount && card.markerType == MarkerType.Archetype)
        {
            playerArchetypeCards[index] = card;
        }
        if(playerFateCards != null && index < maxFateCardAmount && card.markerType == MarkerType.Fate)
        {
            playerFateCards[index] = card;
        }
    }
    public string GetPlayerFateCardString()
    {
        string s = string.Empty;
        for(int i = 0; i < maxFateCardAmount;  i++)
        {
            if (playerFateCards[i] == null) s += "-2|";
            else s += $"{playerFateCards[i].cardId}|";
        }
        return s.Length > 0 ? s.Substring(0,s.Length - 1) : string.Empty;
    }
    public string GetPlayerCardString()
    {
        string s = string.Empty;
        for (int i = 0; i < maxArchetypeCardAmount; i++)
        {
            if (playerArchetypeCards[i] == null) s += "-1|";
            else s += $"{playerArchetypeCards[i].cardId}|";
        }
        return s.Length > 0 ? s.Substring(0, s.Length - 1) : string.Empty;
    }
    public string GetUserFateCardString()
    {
        string s = string.Empty;
        foreach(UserData user in playerAddedRelations)
        {
            for(int i = 0; i < user.maxFateCardAmount; i++)
            {
                if (user.userFateCards[i] == null) s += "-2|";
                else s += $"{user.userFateCards[i].cardId}|";
            }
            s = s.Substring(0, s.Length - 1);
            s += ",";
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
    public string GetUserNoteString()
    {
        NoteWrapper wrapper = new NoteWrapper();
        wrapper.noteList = new List<string>();
        foreach(UserData user in playerAddedRelations)
        {
            string note = user.userNotes;
            wrapper.noteList.Add(note);
        }
        string data = JsonUtility.ToJson(wrapper);
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
    public string GetDiaryEntriesString()
    {
        List<DiaryEntryWrapper> entries = new List<DiaryEntryWrapper>();
        foreach(DiaryEntry entry in diaryEntries)
        {
            DiaryEntryWrapper w = new DiaryEntryWrapper();
            w.text = entry.entryText;
            w.title = entry.entryTitle;
            w.time = entry.entryTime;
            entries.Add(w);
        }
        string data = JsonUtility.ToJson(entries);
        return data;

    }
    public void LoadDiaryEntriesFromString(string loadData)
    {
        if(string.IsNullOrEmpty(loadData)) return;
        List<DiaryEntryWrapper> entries = JsonUtility.FromJson<List<DiaryEntryWrapper>>(loadData);
        foreach(DiaryEntryWrapper entry in entries)
        {
            DiaryEntry loadedEntry = new DiaryEntry(entry.title, entry.text, entry.time);
            diaryEntries.Add(loadedEntry);
        }
    }
    public void LoadPlayerCardsFromString(string loadData)
    {
        Debug.Log($"trying to load player cards {loadData}");
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
    public void LoadPlayerFateCardsFromString(string loadData)
    {
        Debug.Log($"trying to load player fate cards {loadData}");
        if (string.IsNullOrEmpty(loadData)) return;
        string s = string.Empty;
        string[] data = loadData.Split("|");
        int i = 0;
        foreach (string element in data)
        {
            playerFateCards[i] = GameManager.instance.cardDatabase.GetCardByID(int.Parse(element));
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
            int j = 0;
            foreach(string part in parts)
            {
                user.userArchetypeCards[j] = GameManager.instance.cardDatabase.GetCardByID(int.Parse(part));
                j++;
            }
            i++;
        }
    }
    public void LoadUserFateCardsFromString(string loadData)
    {
        if (string.IsNullOrEmpty(loadData)) return;
        string s = string.Empty;
        string[] data = loadData.Split(",");
        int i = 0;
        foreach (UserData user in playerAddedRelations)
        {
            string[] parts = data[i].Split("|");
            int j = 0;
            foreach (string part in parts)
            {
                user.userFateCards[j] = GameManager.instance.cardDatabase.GetCardByID(int.Parse(part));
                j++;
            }
            i++;
        }
    }
    public void LoadUserNoteFromString(string loadData)
    {
        //add id check
        if(string.IsNullOrEmpty(loadData)) return;
        NoteWrapper data = JsonUtility.FromJson<NoteWrapper>(loadData);
        int i = 0;
        foreach(string note in data.noteList)
        {
            playerAddedRelations[i].AddUserNotes(note);
            i++;
        }
    }
}