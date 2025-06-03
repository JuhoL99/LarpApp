using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class ConnectionsPanelManager : MonoBehaviour
{
    [Header("Scroll view content object")]
    [SerializeField] private Transform scrollContentTransform;
    [Header("Prefab")]
    [SerializeField] private GameObject userPanelPrefab;
    [Header("Button")]
    [SerializeField] private Button addUserButton;
    [Header("Username Input Field")]
    [SerializeField] private TMP_InputField nameInputField;
    [Header("Name Warning Text")]
    [SerializeField] private TMP_Text nameWarningText;
    [Header("User Detail Panel")]
    [SerializeField] private GameObject detailViewPanel;
    [SerializeField] private CustomInputField detailViewNotes;
    [SerializeField] private CustomInputField detailViewName;
    [SerializeField] private Card[] detailArchetypeCards;
    [SerializeField] private Card[] detailFateCards;

    private MenuController menuController;


    private ScannerPanelManager scannerPanelManager;
    private List<GameObject> allInstantiatedUserPanels;
    public UnityEvent someoneAddedCard;

    private void Start()
    {
        SaveLoadManager.instance.onGameLoaded.AddListener(LoadExistingUserPanels);
        scannerPanelManager = GetComponent<ScannerPanelManager>();
        scannerPanelManager.onCardAssignRequestToNewUser.AddListener(AddUserPanel);
        scannerPanelManager.onCardAssignRequestToExisitingUser.AddListener(EnableCardAssigning);
        addUserButton.onClick.AddListener(CheckInputFieldName);
        allInstantiatedUserPanels = new List<GameObject>();
        nameInputField.onValueChanged.AddListener(OnInputFieldChanged);
        LoadExistingUserPanels();
    }
    private void EnableCardAssigning(CardSO card)
    {
        Debug.Log("enabled card assigning");
        GameManager.instance.LookingForCardToSelect(card);
    }
    private void OnInputFieldChanged(string txt)
    {
        if (string.IsNullOrWhiteSpace(txt)) nameWarningText.text = "Name empty";
        else if (txt.Length < 1) nameWarningText.text = "Name too short";
        else if (txt.Length > 20) nameWarningText.text = "Name too long";
        else nameWarningText.text = string.Empty;
    }
    private void CheckInputFieldName()
    {
        if (string.IsNullOrWhiteSpace(nameInputField.text)) return;
        if (nameInputField.text.Length == 0) return;
        if (nameInputField.text.Length > 20) return;
        AddUserPanel(nameInputField.text);
        nameInputField.text = string.Empty;
    }
    private void AddUserPanel(CardSO card)
    {
        UserData user = CreateNewUser("New connection", card);
        InstantiateUserPanel(user);
    }
    private void AddUserPanel(string nameInput = "")
    {
        UserData user = CreateNewUser(nameInput);
        InstantiateUserPanel(user);
    }
    private void LoadExistingUserPanels()
    {
        ClearAndDestroyPanels();
        if (GameManager.instance.player == null) return;
        foreach(UserData user in GameManager.instance.player.playerAddedRelations)
        {
            InstantiateUserPanel(user);
        }
    }
    private void InstantiateUserPanel(UserData user)
    {
        GameObject userPanelObject = Instantiate(userPanelPrefab, scrollContentTransform);
        UserPanel panelScript = userPanelObject.GetComponent<UserPanel>();
        panelScript.SetupUserPanel(user, this);
        allInstantiatedUserPanels.Add(userPanelObject);
    }
    private UserData CreateNewUser(string nameInput, CardSO card = null)
    {
        UserData newUser = new UserData(nameInput);
        if (card != null) newUser.AddCardToUser(card, 0);
        GameManager.instance.player.AddUserToRelations(newUser);
        return newUser;
    }
    public void RemoveUser(UserData user, GameObject panel)
    {
        GameManager.instance.player.RemoveUserFromRelations(user);
        allInstantiatedUserPanels.Remove(panel);
        Destroy(panel);
    }
    private void ClearAndDestroyPanels()
    {
        if(allInstantiatedUserPanels.Count == 0) return;
        foreach(GameObject panel in allInstantiatedUserPanels)
        {
            Destroy(panel);
        }
    }
    public void LoadDetails(UserData userToLoad)
    {
        if (menuController == null)
            menuController = gameObject.GetComponent<MenuController>();
        menuController.ShowScreen(detailViewPanel);
        detailViewNotes.AssignUser(userToLoad);
        detailViewNotes.inputField.text = userToLoad.userNotes;
        detailViewName.AssignUser(userToLoad);
        detailViewName.inputField.text = userToLoad.userName;
        for(int i = 0; i < userToLoad.userArchetypeCards.Length; i++)
        {
            detailArchetypeCards[i].SetCurrentCard(userToLoad.userArchetypeCards[i]);
        }
        for(int i = 0; i < userToLoad.userFateCards.Length; i++)
        {
            detailFateCards[i].SetCurrentCard(userToLoad.userFateCards[i]);
        }
    }
}
