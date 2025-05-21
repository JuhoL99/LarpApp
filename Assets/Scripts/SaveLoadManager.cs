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
        PlayerPrefs.DeleteAll();
        string savePath = Path.Combine(Application.persistentDataPath, "save.json");
        using(StreamWriter sw = new StreamWriter(savePath))
        {
            sw.Write(string.Empty);
        }
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
            playerData.GetUserNoteString()
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
        string savePath = Path.Combine(Application.persistentDataPath, "save.json");
        if(!File.Exists(savePath))
        {
            using(StreamWriter writer = new StreamWriter(savePath))
            {
                writer.WriteLine(string.Empty);
            }
        }
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
        playerData.LoadUserNoteFromString(save.linkedUserNotesNew);
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
        onGameSaved?.Invoke();
    }
    private void LoadPlayerPrefs()
    {
        playerData = new PlayerData(PlayerPrefs.GetString("playerName"));
        playerData.LoadUserCardsFromString(PlayerPrefs.GetString("playerCards"));
        playerData.LoadUsersFromString(PlayerPrefs.GetString("linkedUserNames"));
        playerData.LoadUserCardsFromString(PlayerPrefs.GetString("linkedUserCards"));
        GameManager.instance.player = playerData;
        onGameLoaded?.Invoke();
    }
    private void CreateDefaultPlayer()
    {
        playerData = new PlayerData("Player");
        GameManager.instance.player = playerData;
    }
}
