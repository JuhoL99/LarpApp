using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private List<GameObject> allInstantiatedUserPanels;

    private void Start()
    {
        SaveLoadManager.instance.onGameLoaded.AddListener(LoadExistingUserPanels);
        addUserButton.onClick.AddListener(CheckInputFieldName);
        allInstantiatedUserPanels = new List<GameObject>();
        nameInputField.onValueChanged.AddListener(OnInputFieldChanged);
        LoadExistingUserPanels();
    }
    private void OnInputFieldChanged(string txt)
    {
        if (string.IsNullOrWhiteSpace(txt)) nameWarningText.text = "Name empty";
        else if (txt.Length < 1) nameWarningText.text = "Name too short";
        else if (txt.Length > 20) nameWarningText.text = "Name too long";
        //else if (txt.Contains(",") || txt.Contains(":")) nameWarningText.text = "Name can't contain characters ',' and ':'";
        else nameWarningText.text = string.Empty;
    }
    private void CheckInputFieldName()
    {
        if (string.IsNullOrWhiteSpace(nameInputField.text)) return;
        if (nameInputField.text.Length == 0) return;
        if (nameInputField.text.Length > 20) return;
        // (nameInputField.text.Contains(",") || nameInputField.text.Contains(":")) return; //dividers for storing data so wont work in current implementation
        AddUserPanel(nameInputField.text);
        nameInputField.text = string.Empty;
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
    private UserData CreateNewUser(string nameInput)
    {
        UserData newUser = new UserData(nameInput);
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
}
