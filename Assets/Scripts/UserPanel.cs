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
    [Header("Archetype Cards")]
    [SerializeField] private Card[] cards;
    [Header("Fate Cards")]
    [SerializeField] private Card[] fateCards;
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
        foreach(Card card in fateCards)
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
            Debug.Log($"inside userpanel script: {panelUser.userArchetypeCards[i]}");
            cards[i].SetCurrentCard(panelUser.userArchetypeCards[i]);
        }
        for(int i = 0; i < fateCards.Length; i++)
        {
            Debug.Log($"inside userpanel script: {panelUser.userFateCards[i]}");
            fateCards[i].SetCurrentCard(panelUser.userFateCards[i]);
        }
    }
    //rewrite a bit
    private void AddCardToUser(int index, MarkerType type)
    {
        CardSO card = new CardSO();
        if (type == MarkerType.Archetype) card = cards[index].GetCurrentCard();
        else if(type == MarkerType.Fate) card = fateCards[index].GetCurrentCard();
        panelUser.AddCardToUser(card, index);
        manager.someoneAddedCard?.Invoke();
    }
    private void RemoveUser()
    {
        manager.RemoveUser(panelUser, this.gameObject);
    }

}
