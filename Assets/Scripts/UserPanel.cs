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
    [Header("Cards")]
    [SerializeField] private Card[] cards;
    [Header("User Notes")]
    [SerializeField] private InputFieldNotes noteField;

    private void Start()
    {
        addNoteButton.onClick.AddListener(AddNoteToUser);
        removeUserButton.onClick.AddListener(RemoveUser);
        foreach(Card card in cards)
        {
            card.onCardSwitched.AddListener(AddCardToUser);
        }
    }
    public void SetupUserPanel(UserData user, ConnectionsPanelManager mgr)
    {
        panelUser = user;
        manager = mgr;
        noteField.AssignUserToNotes(panelUser);
        UpdatePanelText();
        for(int i = 0; i < cards.Length; i++)
        {
            cards[i].SetCurrentCard(panelUser.userArchetypeCards[i]);
        }
    }
    private void UpdatePanelText()
    {
        panelUserNameText.text = panelUser.userName;
        string noteText = string.Empty;
        //save text straight into input field instead of multiple notes?
        /*foreach(string note in panelUser.notesAboutUser)
        {
            noteText += $"{note} \n";
        }*/
        panelUserNotesText.text = noteText;
    }
    //rewrite a bit
    private void AddCardToUser(int index)
    {
        CardSO card = cards[index].GetCurrentCard(); //not necessary anymore
        panelUser.AddCardToUser(card, index);
        manager.someoneAddedCard?.Invoke();
    }
    private void AddNoteToUser()
    {
        string noteText = noteInputField.text;
        if(noteText == string.Empty) return;
        //panelUser.notesAboutUser.Add(noteText);
        noteInputField.text = string.Empty;
        UpdatePanelText();
    }
    private void RemoveUser()
    {
        manager.RemoveUser(panelUser, this.gameObject);
    }

}
