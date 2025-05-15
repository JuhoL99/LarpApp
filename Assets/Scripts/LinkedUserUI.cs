using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class LinkedUserUI : MonoBehaviour
{
    [SerializeField] private GameObject userPanel;
    [SerializeField] private RectTransform contentParent;
    private List<GameObject> linkedUserObjects;

    private float contentHeight = 0;
    private float userPanelHeight = 0;
    private string csvName;
    private string csvID;
    private void Start()
    {
        contentHeight = contentParent.sizeDelta.y;
        userPanelHeight = userPanel.GetComponent<RectTransform>().sizeDelta.y;
        linkedUserObjects = new List<GameObject>();
        SaveLoadManager.instance.onGameLoaded.AddListener(LoadData);
    }
    public void RemoveLinkedUser(GameObject objectToRemove)
    {
        User userToRemove = objectToRemove.GetComponent<LinkedUserPanel>().thisPanelUser;
        GameManager.instance.player.RemoveRelationFromUser(userToRemove);
        linkedUserObjects.Remove(objectToRemove);
        Destroy(objectToRemove);
    }
    private void LoadData()
    {
        LoadLinkedUsers();
    }
    private void LoadLinkedUsers()
    {
        if (linkedUserObjects.Count != 0)
        {
            foreach(GameObject obj in linkedUserObjects) Destroy(obj);
            linkedUserObjects.Clear();
        }
        foreach(User user in GameManager.instance.player.userAddedRelations)
        {
            GameObject go = Instantiate(userPanel, contentParent);
            LinkedUserPanel lp = go.AddComponent<LinkedUserPanel>();
            lp.thisPanelUser = user;
            lp.linkedUserUI = this;
            linkedUserObjects.Add(go);
        }
        
    }
    public void AddLinkedUser()
    {
        int newUserID = GameManager.instance.player.userAddedRelations.Count + 1;
        User linkedUser = new User("Name", newUserID);
        GameManager.instance.player.AddRelationToUser(linkedUser);
        GameObject go = Instantiate(userPanel, contentParent);
        LinkedUserPanel lp = go.AddComponent<LinkedUserPanel>();
        lp.thisPanelUser = linkedUser;
        lp.linkedUserUI = this;
        linkedUserObjects.Add(go);
    }
    public void SaveTest()
    {
        ShittyCSVConverter();
        PlayerPrefs.SetString("linkedNames", csvName);
        PlayerPrefs.SetString("linkedID", csvID);
        foreach(GameObject go in linkedUserObjects)
        {
            Destroy(go);
        }
        linkedUserObjects.Clear();
    }
    public void LoadTest()
    {
        string[] names = PlayerPrefs.GetString("linkedNames").Split(',');
        string[] idstring = PlayerPrefs.GetString("linkedID").Split(',');
        Debug.Log($"{PlayerPrefs.GetString("linkedNames")} {PlayerPrefs.GetString("linkedID")}");
        int[] ids = Array.ConvertAll(idstring, s=>int.Parse(s));
        LoadExistingLinkedUsers(names.ToArray(), ids.ToArray());

    }
    //do saving and loading in player instead later!
    public void LoadExistingLinkedUsers(string[] names, int[] ids)
    {
        if (names.Length != ids.Length) return;
        for(int i = 0; i < names.Length; i++)
        {
            User linkedUser = new User(names[i], ids[i]);
            GameObject go = Instantiate(userPanel, contentParent);
            LinkedUserPanel lp = go.AddComponent<LinkedUserPanel>();
            lp.thisPanelUser = linkedUser;
            linkedUserObjects.Add(go);
        }
    }
    private void ShittyCSVConverter()
    {
        csvName = "";
        csvID = "";
        foreach (GameObject go in linkedUserObjects)
        {
            LinkedUserPanel linkedUser = go.GetComponent<LinkedUserPanel>();
            csvName += $"{linkedUser.thisPanelUser.userName},";
            csvID += $"{linkedUser.thisPanelUser.userID},";
        }
        csvName = csvName.Substring(0, csvName.Length - 1);
        csvID = csvID.Substring(0,csvID.Length - 1);
    }
}
