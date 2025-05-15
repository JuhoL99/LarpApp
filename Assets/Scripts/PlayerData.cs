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
}
