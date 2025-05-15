using System.Collections.Generic;
using UnityEngine;

public class User
{
    public string userName { get; private set; }
    public int userID { get; private set; }
    public CardSO[] userArchetypeCards { get; private set; }

    public List<User> userAddedRelations { get; private set; }
    public Dictionary<int, string[]> userRelationNotes { get; private set; } //use other user id for key, start from 1 not 0
    public string[] userNotes { get; private set; }

    public User(string name, int id, List<User> addedRelations = null, Dictionary<int, string[]> relationNotes = null)
    {
        userName = name;
        userID = id;
        userArchetypeCards = new CardSO[2];
        if(addedRelations == null) userAddedRelations = new List<User>();
        else userAddedRelations = addedRelations;
        if(relationNotes == null) userRelationNotes = new Dictionary<int, string[]>();
        else userRelationNotes = relationNotes;
    }
    public void AddCardToUser(CardSO card, int slot)
    {
        userArchetypeCards[slot] = card;
    }
    public void SetUserCards(CardSO[] userCards)
    {
        userArchetypeCards = userCards;
    }
    public void AddRelationToUser(User userToAdd)
    {
        if(userToAdd == this) return;
        userAddedRelations.Add(userToAdd);
    }
    public void SetRelationNotes(Dictionary<int, string[]> relationNotes)
    {
        userRelationNotes = relationNotes;
    }
    public void RemoveRelationFromUser(User userToRemove)
    {
        if(userToRemove == this) return;
        userAddedRelations.Remove(userToRemove);
        /*foreach (var user in userAddedRelations)
        {
            if (user.userID == userToRemove.userID) userAddedRelations.Remove(userToRemove);
        }*/
    }
    public void ChangeUserName(User user, string newName)
    {
        user.userName = newName;
    }
    
}
