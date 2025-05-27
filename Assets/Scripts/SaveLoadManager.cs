using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.IO;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager instance;
    [Header("Save on application quit")]
    [SerializeField] private bool saveOnQuit;
    [Header("Autosave")] //just in case?
    [SerializeField] private bool autoSave;
    [SerializeField] private float autoSaveFreq;
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
        if(autoSave) InvokeRepeating("Save", autoSaveFreq, autoSaveFreq);
    }
    private void OnApplicationPause(bool pause)
    {
#if UNITY_EDITOR
        return;
#elif UNITY_ANDROID || UNITY_IOS
        if (saveOnQuit) Save();
#endif
    }
    private void OnApplicationQuit()
    {
        if (saveOnQuit) Save();
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
            playerData.playerNotes,
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
        playerData = new PlayerData(save.playerName, save.playerNotes);
        playerData.LoadUserCardsFromString(save.playerCards);
        playerData.LoadUsersFromString(save.linkedUserNames);
        playerData.LoadUserCardsFromString(save.linkedUserCards);
        playerData.LoadUserNoteFromString(save.linkedUserNotesNew);
        GameManager.instance.player = playerData;
        onGameLoaded?.Invoke();
    }
    private void CreateDefaultPlayer()
    {
        playerData = new PlayerData("Player");
        GameManager.instance.player = playerData;
    }
}
