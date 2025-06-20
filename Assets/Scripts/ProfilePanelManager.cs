using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ProfilePanelManager : MonoBehaviour
{
    private ScannerPanelManager scannerPanel;
    [Header("Profile Panel")]
    [SerializeField] private GameObject profilePanel;
    [Header("Profile Archetype Cards")]
    [SerializeField] private Card[] archetypeCards = new Card[2];
    [Header("Additional Archetype Displays")]
    [SerializeField] private Card[] linkedArchetypeCards = new Card[2];
    [Header("Profile Fate Cards")]
    [SerializeField] private Card[] fateCards = new Card[3];
    [Header("Card Added Event")]
    public UnityEvent cardAddedToProfile;
    [Header("Input Fields")]
    [SerializeField] private CustomInputField playerNameField;
    [SerializeField] private CustomInputField playerNoteField;
    private void Awake()
    {
        scannerPanel = GetComponent<ScannerPanelManager>();
    }
    private void Start()
    {
        scannerPanel.onCardAssignRequestToPlayer.AddListener(CardAssignRequestToProfile);
        StartCoroutine(LateStart()); //because load happens in start so player doesnt exist yet
    }
    private IEnumerator LateStart()
    {
        yield return null;
        if (playerNameField != null) playerNameField.AssignPlayer(GameManager.instance.player);
        if (playerNoteField != null) playerNoteField.AssignPlayer(GameManager.instance.player);
        int i = 0;
        foreach (var card in archetypeCards)
        {
            card.onCardSwitched.AddListener(CardAdded);
            card.SetCurrentCard(
                GameManager.instance?.player?.playerArchetypeCards[i] == null ?
                GameManager.instance?.cardDatabase?.GetCardByID(-1) :
                GameManager.instance?.player?.playerArchetypeCards[i]
                );
            i++;
        }
        i = 0;
        foreach (var card in fateCards)
        {
            card.onCardSwitched.AddListener(CardAdded);
            card.SetCurrentCard(
                GameManager.instance?.player?.playerFateCards[i] == null ?
                GameManager.instance?.cardDatabase?.GetCardByID(-2) :
                GameManager.instance?.player?.playerFateCards[i]
                );
            i++;
        }

        // Link the cards on main screen to display the same cards as profile panel
        for (int j = 0; j < archetypeCards.Length && j < linkedArchetypeCards.Length; j++)
        {
            if (linkedArchetypeCards[j] != null && archetypeCards[j] != null)
            {
                linkedArchetypeCards[j].SetCurrentCard(archetypeCards[j].GetCurrentCard());

                int capturedIndex = j;
                archetypeCards[j].onCardSwitched.AddListener((idx, type) => {
                    linkedArchetypeCards[capturedIndex].SetCurrentCard(archetypeCards[capturedIndex].GetCurrentCard());
                });
            }
        }
    }
    private void CardAssignRequestToProfile(CardSO card)
    {
        if(!profilePanel.activeSelf) profilePanel.SetActive(true);
        GameManager.instance.LookingForCardToSelect(card);
    }
    private void CardAdded(int index, MarkerType type)
    {
        if(type == MarkerType.Archetype) GameManager.instance.player.AddCardToPlayer(archetypeCards[index].GetCurrentCard(),index);
        else GameManager.instance.player.AddCardToPlayer(fateCards[index].GetCurrentCard(),index);
        cardAddedToProfile?.Invoke();
    }
}
