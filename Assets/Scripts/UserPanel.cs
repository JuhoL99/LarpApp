using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserPanel : MonoBehaviour
{
    private UserData panelUser;
    private ConnectionsPanelManager manager;
    [Header("Text Components")]
    [SerializeField] private TMP_Text panelUserNameText;
    [SerializeField] private TMP_Text panelUserNotesText;
    [Header("Buttons")]
    [SerializeField] private Button addNoteButton;
    [SerializeField] private Button removeUserButton;
    [Header("Note Input Field")]
    [SerializeField] private TMP_InputField noteInputField;

    private void Start()
    {
        addNoteButton.onClick.AddListener(AddNoteToUser);
        removeUserButton.onClick.AddListener(RemoveUser);
    }
    public void SetupUserPanel(UserData user, ConnectionsPanelManager mgr)
    {
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
        noteInputField.text = string.Empty;
        UpdatePanelText();
    }
    private void RemoveUser()
    {
        manager.RemoveUser(panelUser, this.gameObject);
    }

}
