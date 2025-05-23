using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserPanel : MonoBehaviour
{
    private UserData panelUser;
    private ConnectionsPanelManager manager;
    [Header("Buttons")]
    [SerializeField] private Button addNoteButton;
    [SerializeField] private Button removeUserButton;
    [Header("Note Input Field")]
    [SerializeField] private TMP_InputField noteInputField;
    [Header("Cards")]
    [SerializeField] private Card[] cards;
    [Header("User Notes")]
    [SerializeField] private CustomInputFieldNotes noteField;
    [Header("User Name")]
    [SerializeField] private CustomInputFieldName nameField;

    private void Start()
    {
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
        nameField.AssignUser(panelUser);
        noteField.AssignUser(panelUser);
        for(int i = 0; i < cards.Length; i++)
        {
            cards[i].SetCurrentCard(panelUser.userArchetypeCards[i]);
        }
    }
    //rewrite a bit
    private void AddCardToUser(int index)
    {
        CardSO card = cards[index].GetCurrentCard();
        panelUser.AddCardToUser(card, index);
        manager.someoneAddedCard?.Invoke();
    }
    private void RemoveUser()
    {
        manager.RemoveUser(panelUser, this.gameObject);
    }

}
