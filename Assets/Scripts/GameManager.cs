using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using TMPro;
//just a general place to access different scripts and more
public class GameManager : MonoBehaviour
{
    [Header("Scripts/Gameobjects")]
    public static GameManager instance;
    public CardDatabase cardDatabase;
    public CardScanner cardScanner;
    public PlayerData player;
    public ConnectionsPanelManager connectionPanelManager;
    public ProfilePanelManager profilePanelManager;
    public ScannerPanelManager scannerPanelManager;
    public GameObject cardPopup;
    [Header("Flags")]
    public bool isLookingForCardToSelect = false;
    [Header("Info")]
    public CardSO currentScannedCard;
    [Header("Testing")]
    [SerializeField] private bool generateNamesForScriptableObjects = false; //temporary
    [SerializeField] private bool generateUsersFromStart = false;
    //private TMP_Text fpsText;
    private void Awake()
    {
        if (instance == null) instance = this;
        if(generateNamesForScriptableObjects) cardDatabase.NamesFromImageFile();
        RefreshRate refreshRate = Screen.currentResolution.refreshRateRatio;
        Application.targetFrameRate = refreshRate.value > 0 ? Mathf.RoundToInt((float)refreshRate.value) : 60;
    }
    private void Start()
    {
        //disable card switch option when card selection detected
        connectionPanelManager.someoneAddedCard.AddListener(() => isLookingForCardToSelect = false);
        profilePanelManager.cardAddedToProfile.AddListener(() => isLookingForCardToSelect = false);
        if(generateUsersFromStart) StartCoroutine(LateStart());
        //fpsText = GetComponentInChildren<TMP_Text>();
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
    //when card is clicked after this is called, change that cards
    public void LookingForCardToSelect(CardSO card)
    {
        isLookingForCardToSelect = true;
        currentScannedCard = card;
    }
}
