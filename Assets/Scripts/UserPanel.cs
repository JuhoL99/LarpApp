using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserPanel : MonoBehaviour
{
    private UserData panelUser;
    private UserPanelManager manager;
    private TMP_Text panelUserNameText;
    private TMP_Text panelUserNotesText;
    private Button addNoteButton;
    private Button removeUserButton;
    private TMP_InputField noteInputField;

    private void Start()
    {
        addNoteButton = transform.GetChild(2).GetComponent<Button>();
        removeUserButton = transform.GetChild(3).GetComponent<Button>();
        noteInputField = transform.GetChild(4).GetComponent<TMP_InputField>();
        addNoteButton.onClick.AddListener(AddNoteToUser);
        removeUserButton.onClick.AddListener(RemoveUser);
    }
    public void SetupUserPanel(UserData user, UserPanelManager mgr)
    {
        panelUserNameText = transform.GetChild(0).GetComponent<TMP_Text>();
        panelUserNotesText = transform.GetChild(1).GetComponent<TMP_Text>();
        panelUser = user;
        manager = mgr;
        UpdatePanelText();
    }
    private void UpdatePanelText()
    {
        panelUserNameText.text = panelUser.userName;
        string noteText = string.Empty;
        foreach(string note in panelUser.notesAboutUser)
        {
            Debug.Log($"{panelUser.userName} notes {note}");
            noteText += $"{note} \n";
        }
        panelUserNotesText.text = noteText;
    }
    private void AddNoteToUser()
    {
        string noteText = noteInputField.text;
        if(noteText == string.Empty) return;
        panelUser.notesAboutUser.Add(noteText);
        UpdatePanelText();
    }
    private void RemoveUser()
    {
        manager.RemoveUser(panelUser, this.gameObject);
    }

}
