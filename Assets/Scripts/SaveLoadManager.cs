using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.IO;
using UnityEngine.SceneManagement;

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
        if (autoSave) InvokeRepeating("Save", autoSaveFreq, autoSaveFreq);
    }
    //android doesnt always invoke "on application close" but pause runs when closed
    private void OnApplicationPause(bool pause)
    {
#if UNITY_EDITOR
        return;
#elif UNITY_ANDROID || UNITY_IOS
        if (saveOnQuit && pause) Save();
#endif
    }
    private void OnApplicationQuit()
    {
        if (saveOnQuit) Save();
    }
    //in case you quickly want to delete save file
    public void ClearAllData()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "save.json");
        using(StreamWriter sw = new StreamWriter(savePath))
        {
            sw.Write(string.Empty);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    //call from anywhere you need
    public void Save()
    {
        SaveToFile();
    }
    public void Load()
    {
        LoadFromFile();
    }
    //turns all the info in PlayerData class to a Save object and converts it to json
    private void SaveToFile()
    {
        if (playerData == null) playerData = GameManager.instance.player;
        Debug.Log(playerData.GetDiaryEntriesString());
        Save save = new Save(
            playerData.playerName,
            playerData.playerNotes,
            playerData.GetPlayerCardString(),
            playerData.GetPlayerFateCardString(),
            playerData.GetUserRelationsString(),
            playerData.GetUserCardString(),
            playerData.GetUserFateCardString(),
            playerData.GetUserNoteString(),
            playerData.GetDiaryEntriesString()
            );
        string text = JsonUtility.ToJson(save);
        string savePath = Path.Combine(Application.persistentDataPath, "save.json");
        using(StreamWriter write = new StreamWriter(savePath))
        {
            write.Write(text);
        }
    }
    //turns json Save object back into player data
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
        if (string.IsNullOrWhiteSpace(text))
        {
            Debug.Log("text null or space");
            CreateDefaultPlayer();
            onGameLoaded?.Invoke(); 
            return;
        }
        Save save = JsonUtility.FromJson<Save>(text);
        playerData = new PlayerData(save.playerName, save.playerNotes);
        playerData.LoadPlayerCardsFromString(save.playerCards);
        playerData.LoadUsersFromString(save.linkedUserNames);
        playerData.LoadUserCardsFromString(save.linkedUserCards);
        playerData.LoadUserNoteFromString(save.linkedUserNotesNew);
        playerData.LoadPlayerFateCardsFromString(save.playerFateCards);
        playerData.LoadUserFateCardsFromString(save.linkedUserFateCards);
        playerData.LoadDiaryEntriesFromString(save.diaryEntries);
        GameManager.instance.player = playerData;
        onGameLoaded?.Invoke();
    }
    private void CreateDefaultPlayer()
    {
        playerData = new PlayerData("Player");
        GameManager.instance.player = playerData;
    }
}
