using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using Random = UnityEngine.Random;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CardDatabase cardDatabase;
    public CardScanner cardScanner;
    public PlayerData player;
    [Header("Testing")]
    [SerializeField] private bool generateNamesForScriptableObjects = false; //temporary
    [SerializeField] private bool generateUsersFromStart = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        if(generateNamesForScriptableObjects) cardDatabase.NamesFromImageFile();
    }
    private void Start()
    {
       if(generateUsersFromStart) StartCoroutine(LateStart());
    }
    private IEnumerator LateStart()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 10; i++)
        {
            UserData user = new UserData($"name{i}");
            user.AddCardToUser(cardDatabase.GetCardByID(Random.Range(0, 69)), 0);
            user.AddCardToUser(cardDatabase.GetCardByID(Random.Range(0, 69)), 1);
            user.AddNoteToUser(
                "jfakjkהההההההההההההההההההההההההההההההההההההההההההההההההההההההההההההה" +
                "הההההההההההההההההההההההההההההההההההההההההההההההההההההההההההההההההההה" +
                "צצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצצ" +
                "6&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&" +
                ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,," +
                "...................................................................."
                );
            player.AddUserToRelations(user);
        }
    }
}
