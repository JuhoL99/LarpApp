using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserPanelManager : MonoBehaviour
{
    [Header("Scroll view content object")]
    [SerializeField] private Transform scrollContentTransform;
    [Header("Prefab")]
    [SerializeField] private GameObject userPanelPrefab;
    [Header("Button")]
    [SerializeField] private Button addUserButton;
    [Header("Username Input Field")]
    [SerializeField] private TMP_InputField nameInputField;
    private List<GameObject> allInstantiatedUserPanels;

    private void Start()
    {
        SaveLoadManager.instance.onGameLoaded.AddListener(LoadExistingUserPanels);
        addUserButton.onClick.AddListener(CheckInputFieldName);
        allInstantiatedUserPanels = new List<GameObject>();
    }
    private void CheckInputFieldName()
    {
        if (nameInputField.text.Length == 0) return;
        if (nameInputField.text.Length > 20) return;
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
        if(GameManager.instance.player.playerAddedRelations != null && GameManager.instance.player.playerAddedRelations.Count > 0)
        foreach(UserData user in GameManager.instance.player.playerAddedRelations)
        {
            InstantiateUserPanel(user);
        }
    }
    private void InstantiateUserPanel(UserData user)
    {
        GameObject userPanelObject = Instantiate(userPanelPrefab, scrollContentTransform);
        UserPanel panelScript = userPanelObject.AddComponent<UserPanel>();
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
        foreach(GameObject panel in allInstantiatedUserPanels)
        {
            Destroy(panel);
        }
    }
}
