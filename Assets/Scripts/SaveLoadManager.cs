using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager instance;
    public UnityEvent onGameSaved;
    public UnityEvent onGameLoaded;
    private PlayerData playerData;
    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        Load();
    }
    public void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
    }
    public void Save()
    {
        //use file instead of playerprefs later
        SavePlayerPrefs();
    }
    public void Load()
    {
        LoadPlayerPrefs();
    }
    private void SavePlayerPrefs()
    {
        if (playerData == null) playerData = GameManager.instance.player;
        PlayerPrefs.SetString("playerName", playerData.playerName);
        PlayerPrefs.SetString("playerCards", playerData.GetPlayerCardString());
        PlayerPrefs.SetString("linkedUserNames", playerData.GetUserRelationsString());
        PlayerPrefs.SetString("linkedUserCards", playerData.GetUserCardString());
        PlayerPrefs.SetString("linkedUserNotes", playerData.GetUserNotesString());
        onGameSaved?.Invoke();
    }
    private void LoadPlayerPrefs()
    {
        playerData = new PlayerData(PlayerPrefs.GetString("playerName"));
        playerData.LoadUserCardsFromString(PlayerPrefs.GetString("playerCards"));
        Debug.Log(PlayerPrefs.GetString("linkedUserNames"));
        playerData.LoadUsersFromString(PlayerPrefs.GetString("linkedUserNames"));
        Debug.Log(PlayerPrefs.GetString("linkedUserCards"));
        playerData.LoadUserCardsFromString(PlayerPrefs.GetString("linkedUserCards"));
        playerData.LoadUserNotesFromString(PlayerPrefs.GetString("linkedUserNotes"));
        GameManager.instance.player = playerData;
        onGameLoaded?.Invoke();
    }
}
