using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
public class GameManager : MonoBehaviour
{
    [Header("Scripts")]
    public static GameManager instance;
    public CardDatabase cardDatabase;
    public CardScanner cardScanner;
    public WindowManager windowManager;
    public PlayerData player;
    public ConnectionsPanelManager connectionPanelManager;
    [Header("Flags")]
    public bool isLookingForCardToSelect = false;
    [Header("Info")]
    public CardSO currentScannedCard;
    [Header("Testing")]
    [SerializeField] private bool generateNamesForScriptableObjects = false; //temporary
    [SerializeField] private bool generateUsersFromStart = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        if(generateNamesForScriptableObjects) cardDatabase.NamesFromImageFile();
    }
    private void Start()
    {
        connectionPanelManager.someoneAddedCard.AddListener(() => isLookingForCardToSelect = false);
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
            user.AddUserNotes("notenotenotenotenotenotenotenotenotenotenotenotenotenotenotenotenotenotenotenotenotenotenotenotenotenotenotenote");
            player.AddUserToRelations(user);
        }
    }
    public void LookingForCardToSelect(CardSO card)
    {
        isLookingForCardToSelect = true;
        currentScannedCard = card;
    }
}
