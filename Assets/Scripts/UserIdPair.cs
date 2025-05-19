using System;
using UnityEngine;

[System.Serializable]
public class UserIdPair
{
    public string userName;
    public Guid userID;

    public UserIdPair(string userName, Guid userID)
    {
        this.userName = userName;
        this.userID = userID;
    }
}
