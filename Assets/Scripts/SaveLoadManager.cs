using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.IO;

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
        //PlayerPrefs.DeleteAll();
        string savePath = Path.Combine(Application.persistentDataPath, "save.json");
        StreamWriter writer = new StreamWriter(savePath);
        writer.Write(string.Empty);
        writer.Close();

    }
    public void Save()
    {
        SaveToFile();
    }
    public void Load()
    {
        LoadFromFile();
    }
    private void SaveToFile()
    {
        if (playerData == null) playerData = GameManager.instance.player;
        Save save = new Save(
            playerData.playerName,
            playerData.GetPlayerCardString(),
            playerData.GetUserRelationsString(),
            playerData.GetUserCardString(),
            playerData.GetUserNotesString()
            );
        string text = JsonUtility.ToJson(save);
        string savePath = Path.Combine(Application.persistentDataPath, "save.json");
        using(StreamWriter write = new StreamWriter(savePath))
        {
            write.Write(text);
        }
    }
    private void LoadFromFile()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "save.json"); //add check if file exists later
        string text;
        using(StreamReader reader = new StreamReader(savePath))
        {
            text = reader.ReadToEnd();
        }
        if (string.IsNullOrEmpty(text))
        { 
            CreateDefaultPlayer();
            onGameLoaded?.Invoke(); 
            return;
        }
        Save save = JsonUtility.FromJson<Save>(text);
        playerData = new PlayerData(save.playerName);
        playerData.LoadUserCardsFromString(save.playerCards);
        playerData.LoadUsersFromString(save.linkedUserNames);
        playerData.LoadUserCardsFromString(save.linkedUserCards);
        playerData.LoadUserNotesFromString(save.linkedUserNotes);
        GameManager.instance.player = playerData;
        onGameLoaded?.Invoke();

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
        playerData.LoadUsersFromString(PlayerPrefs.GetString("linkedUserNames"));
        playerData.LoadUserCardsFromString(PlayerPrefs.GetString("linkedUserCards"));
        playerData.LoadUserNotesFromString(PlayerPrefs.GetString("linkedUserNotes"));
        GameManager.instance.player = playerData;
        onGameLoaded?.Invoke();
    }
    private void CreateDefaultPlayer()
    {
        playerData = new PlayerData("Player");
        GameManager.instance.player = playerData;
    }
}
