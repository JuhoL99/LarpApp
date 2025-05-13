using System.Collections.Generic;
using UnityEngine;

public class User
{
    public string userName { get; private set; }
    public int userID { get; private set; }
    public CardSO[] userArchetypeCards { get; private set; }

    public List<User> userAddedRelations { get; private set; }
    public Dictionary<int, string[]> userRelationNotes { get; private set; }
    public string[] userNotes { get; private set; }

    public User(string name, int id)
    {
        userName = name;
        userID = id;
    }
    public void AddCardToUser(CardSO card, int slot)
    {
        userArchetypeCards[slot] = card;
    }
    public void AddRelationToUser(User userToAdd)
    {
        if(userToAdd == this) return;
        userAddedRelations.Add(userToAdd);
    }
    public void RemoveRelationFromUser(User userToRemove)
    {
        if(userToRemove == this) return;
        foreach (var user in userAddedRelations)
        {
            if (user.userID == userToRemove.userID) userAddedRelations.Remove(userToRemove);
        }
    }
    public void ChangeUserName(User user, string newName)
    {
        user.userName = newName;
    }
}
