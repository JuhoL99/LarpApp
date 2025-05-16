using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CardDatabase cardDatabase;
    public CardScanner cardScanner;
    public User player;
    [SerializeField] private bool generateNamesForScriptableObjects = false; //temporary

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        if(generateNamesForScriptableObjects) cardDatabase.NamesFromImageFile(); //remove l8r
    }
    private void Start()
    {
        PlayerData p = new PlayerData("playername");
        /*for (int i = 0; i < 10; i++)
        {
            UserData user = new UserData($"name {i}");
            p.AddUserToRelations(user);
        }*/
        string l = "9af92991-fef9-4c28-89dd-ea28af686c71:name 0,c9c9d511-218a-465c-a939-edf25439cee3:name 1,8db078ec-72c6-4489-aa6f-2f7fa1ad0f8f:name 2,fd6ff51e-935e-4972-a2f8-45559bd92cf3:name 3,81a0aec7-1167-4946-bea8-31224e7b1d7b:name 4,3eab6af6-2685-49a4-b512-81a85681f27e:name 5,b9bc7e2e-3678-4863-838c-6751b6726fe1:name 6,3b1d543f-9d4c-48ca-973b-3d8700e9ef87:name 7,5aa35b0c-c1f9-4946-95ba-b616bf358462:name 8,fdc2d2b5-8341-4ebb-af0c-6a2078c65579:name 9";
        p.LoadUsersFromString(l);
        Debug.Log($"loaded string: {l}");
        Debug.Log(p.GetUserRelationsString());
        Debug.Log(p.GetUserCardString());
        //temp
        player = new User("Player", 0);
        return;
        for(int i = 0; i < 10; i++)
        {
            User user = new User($"user {i + 1}", i + 1);
            player.AddRelationToUser(user);
        }
    }
}
