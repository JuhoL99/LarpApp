using System.Collections.Generic;
using UnityEngine;

public class User
{
    public string userName { get; private set; }
    public int userID { get; private set; }
    public CardSO[] userArchetypeCards { get; private set; }

    public List<User> userAddedRelations { get; private set; }
    public string[] userNotes { get; private set; }

    public User(string name, int id)
    {
        userName = name;
        userID = id;
    }
}
