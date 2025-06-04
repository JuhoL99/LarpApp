using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Events;
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

    [Header("Temporary Testing Text")]
    [SerializeField] private TMP_Text posx;
    [SerializeField] private TMP_Text posy;
    [SerializeField] private TMP_Text posz;
    [SerializeField] private TMP_Text width;
    [SerializeField] private TMP_Text height;
    [SerializeField] private TMP_Text anchorx;
    [SerializeField] private TMP_Text anchory;



    private InputAction backAction = new InputAction("Back", InputActionType.Button, "<Keyboard>/escape");
    public UnityEvent onBackAction;

    private void Awake()
    {
        if (instance == null) instance = this;
        if(generateNamesForScriptableObjects) cardDatabase.NamesFromImageFile();
        RefreshRate refreshRate = Screen.currentResolution.refreshRateRatio;
        Application.targetFrameRate = refreshRate.value > 0 ? Mathf.RoundToInt((float)refreshRate.value) : 60;
    }
    private void Start()
    {
        backAction.Enable();
        backAction.performed += HandleInput;

        //disable card switch option when card selection detected
        connectionPanelManager.someoneAddedCard.AddListener(() => isLookingForCardToSelect = false);
        profilePanelManager.cardAddedToProfile.AddListener(() => isLookingForCardToSelect = false);
        if(generateUsersFromStart) StartCoroutine(LateStart());
    }
    private void HandleInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            onBackAction?.Invoke();
            if(cardPopup.activeInHierarchy) cardPopup.SetActive(false);
        }
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

    private void Update()
    {
        TestFunction();
    }
    private void TestFunction()
    {
        height.text = $"height: {GetKeyboardSize()}";
    }
    public int GetKeyboardSize()
    {
#if UNITY_ANDROID
        using (AndroidJavaClass UnityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject View = UnityClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer").Call<AndroidJavaObject>("getView");

            using (AndroidJavaObject Rct = new AndroidJavaObject("android.graphics.Rect"))
            {
                View.Call("getWindowVisibleDisplayFrame", Rct);

                return Screen.height - Rct.Call<int>("height");
            }
        }
#endif
    }
}
