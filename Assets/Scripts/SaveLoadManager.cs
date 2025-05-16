using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager instance;
    public UnityEvent onGameSaved;
    public UnityEvent onGameLoaded;
    private User oldPlayer;
    private PlayerData playerData;
    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        oldPlayer = GameManager.instance.player;
    }
    public void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
    }
    public void Save()
    {
        SavePlayerPrefs();
    }
    public void Load()
    {
        LoadPlayerPrefs();
    }
    private void SavePlayerPrefs()
    {
        PlayerPrefs.SetString("playerName", playerData.playerName);
        PlayerPrefs.SetString("playerCards", playerData.GetPlayerCardString());
        PlayerPrefs.SetString("linkedUserNames", playerData.GetUserRelationsString());
        PlayerPrefs.SetString("linkedUserCards", playerData.GetUserCardString());
        PlayerPrefs.SetString("linkedUserNotes", playerData.GetUserNotesString());
    }
    private void LoadPlayerPrefs()
    {
        playerData = new PlayerData(playerData.playerName);
        playerData.LoadUserCardsFromString(PlayerPrefs.GetString("playerCards"));
        playerData.LoadUsersFromString(PlayerPrefs.GetString("linkedUserNames"));
        playerData.LoadUserCardsFromString(PlayerPrefs.GetString("linkedUserCards"));
        playerData.LoadUserNotesFromString(PlayerPrefs.GetString("linkedUserNotes"));
        GameManager.instance.p = playerData;
    }
    private void SavePlayerPrefsOld()
    {
        if (oldPlayer == null) return;
        string playerName = oldPlayer.userName;
        string playerCards = CardsToCSV(oldPlayer.userArchetypeCards);
        string linkedNames = RelationsToCSV(oldPlayer.userAddedRelations, 0);
        string linkedIDs = RelationsToCSV(oldPlayer.userAddedRelations,1);
        string linkedCards = RelationsToCSV(oldPlayer.userAddedRelations, 2);
        string linkedNotes = RelationsToCSV(oldPlayer.userAddedRelations, 3);
        PlayerPrefs.SetString("playerName", playerName);
        PlayerPrefs.SetString("playerCards", playerCards);
        PlayerPrefs.SetString("linkedNames", linkedNames);
        PlayerPrefs.SetString("linkedIDs", linkedIDs);
        PlayerPrefs.SetString("linkedCards", linkedCards);
        PlayerPrefs.SetString("linkedNotes", linkedNotes);
        onGameSaved?.Invoke();
    }
    private void LoadPlayerPrefsOld()
    {
        string playerName = PlayerPrefs.GetString("playerName", "Player");
        string playerCardsData = PlayerPrefs.GetString("playerCards", "");
        string linkedNamesData = PlayerPrefs.GetString("linkedNames", "");
        string linkedIDsData = PlayerPrefs.GetString("linkedIDs", "");
        string linkedCardsData = PlayerPrefs.GetString("linkedCards", "");
        string linkedNotesData = PlayerPrefs.GetString("linkedNotes", "");

        User loadedPlayer = new User(playerName, 0);
        List<User> linkedUsers = new List<User>();

        string[] playerCardIDs = !string.IsNullOrEmpty(playerCardsData) ? playerCardsData.Split(',') : new string[0];
        CardSO[] playerCards = new CardSO[2];
        for (int i = 0; i < 2 && i < playerCardIDs.Length; i++)
        {
            if (int.TryParse(playerCardIDs[i], out int cardId) && cardId != -1)
            {
                playerCards[i] = GameManager.instance.cardDatabase.GetCardByID(cardId);
            }
        }
        loadedPlayer.SetUserCards(playerCards);

        if (!string.IsNullOrEmpty(linkedNamesData) && !string.IsNullOrEmpty(linkedIDsData))
        {
            string[] linkedNames = linkedNamesData.Split(',');
            string[] linkedIDs = linkedIDsData.Split(',');
            int userCount = Mathf.Min(linkedNames.Length, linkedIDs.Length);

            for (int i = 0; i < userCount; i++)
            {
                if (int.TryParse(linkedIDs[i], out int userId))
                {
                    User user = new User(linkedNames[i], userId);
                    linkedUsers.Add(user);
                    loadedPlayer.AddRelationToUser(user);
                }
            }
        }

        if (!string.IsNullOrEmpty(linkedCardsData) && linkedUsers.Count > 0)
        {
            string[] userCardsSets = linkedCardsData.Split('|');

            for (int i = 0; i < linkedUsers.Count && i < userCardsSets.Length; i++)
            {
                string[] cardIds = userCardsSets[i].Split(',');
                CardSO[] userCards = new CardSO[2];

                for (int j = 0; j < 2 && j < cardIds.Length; j++)
                {
                    if (int.TryParse(cardIds[j], out int cardId) && cardId != -1)
                    {
                        userCards[j] = GameManager.instance.cardDatabase.GetCardByID(cardId);
                    }
                }

                linkedUsers[i].SetUserCards(userCards);
            }
        }

        Dictionary<int, string[]> relationNotes = new Dictionary<int, string[]>();
        if (!string.IsNullOrEmpty(linkedNotesData))
        {
            string[] entries = linkedNotesData.Split(';');

            foreach (string entry in entries)
            {
                string[] parts = entry.Split(':');
                if (parts.Length >= 1 && int.TryParse(parts[0], out int userId))
                {
                    string[] notes = parts.Length > 1 && !string.IsNullOrEmpty(parts[1])
                        ? parts[1].Split(',')
                        : new string[0];

                    relationNotes.Add(userId, notes);
                }
            }
        }
        loadedPlayer.SetRelationNotes(relationNotes);

        GameManager.instance.player = loadedPlayer;
        oldPlayer = loadedPlayer;
        onGameLoaded?.Invoke();
    }

    private string CardsToCSV(CardSO[] cards)
    {
        string csv = "";
        for (int i = 0; i < 2; i++)
        {
            CardSO card = (i < cards.Length) ? cards[i] : null;
            if (card != null)
            {
                csv += $"{card.cardId},";
            }
            else
            {
                csv += "-1,";
            }
        }
        Debug.Log(csv);
        return csv.Substring(0, csv.Length - 1);
    }
    private string RelationsToCSV(List<User> linkedUsers, int type) //0 = names, 1 = ids, 2 = cards, 3 = notes
    {
        string csv = "";
        foreach(User user in linkedUsers)
        {
            if(type == 0)
            {
                csv += $"{user.userName},";
            }
            if(type == 1)
            {
                csv += $"{user.userID},";
            }
            if(type == 2)
            {
                csv = RelationCardsToCSV();
            }
            if(type == 3)
            {
                csv = UserNotesToCSV();
            }
        }
        Debug.Log(csv);
        return csv.Substring(0, csv.Length - 1);
    }
    private string RelationCardsToCSV()
    {
        string csv = "";
        foreach (User user in oldPlayer.userAddedRelations)
        {
            if (!string.IsNullOrEmpty(csv)) csv += "|";
            for (int i = 0; i < 2; i++)
            {
                int cardId = -1;
                if (user.userArchetypeCards != null && i < user.userArchetypeCards.Length && user.userArchetypeCards[i] != null)
                {
                    cardId = user.userArchetypeCards[i].cardId;
                }
                csv += cardId.ToString();

                if (i == 0) csv += ",";
            }
        }
        return csv;
    }
    private string UserNotesToCSV()
    {
        Dictionary<int, string[]> relationNotes = oldPlayer.userRelationNotes;
        if (relationNotes == null || relationNotes.Count == 0)
            return "";

        List<string> entries = new List<string>();

        foreach(var kvp in relationNotes)
        {
            int id = kvp.Key;
            string[] notes = kvp.Value;
            if(notes == null || notes.Length == 0)
            {
                entries.Add(id.ToString()+":");
                continue;
            }
            string notesString = string.Join(",", notes);
            entries.Add(id.ToString() + ":"+notesString);
        }
        return string.Join(";", entries);
    }
}
