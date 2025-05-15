using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public string playerName;
    public CardSO[] playerArchetypeCards;
    public List<UserData> playerAddedRelations;
    private int maxCardAmount = 2;

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
    public string GetUserCardsString()
    {
        string s = string.Empty;
        for (int i = 0; i < maxCardAmount; i++)
        {
            if (playerArchetypeCards[i] == null) s += "-1|";
            else s += $"{playerArchetypeCards[i].cardId}|";
        }
        return s = s.Substring(0, s.Length - 1);
    }
    public string GetUserRelationsString()
    {
        string s = string.Empty;

        foreach(UserData user in playerAddedRelations)
        {
            s += $"{user.userID}:{user.userName},";
        }
        return s = s.Substring(0, s.Length-1);
    }
    public List<UserData> LoadUsersFromString(string loadData)
    {
        List<UserData> users = new List<UserData>();
        string[] data = loadData.Split(",");
        foreach (var element in data)
        {
            string[] parts = element.Split(":");
            UserData user = new UserData(parts[1]);
            this.AddUserToRelations(user);
        }
        return users;

    }
}
