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
    [Header("Card Added Event")]
    public UnityEvent cardAddedToProfile;
    [Header("Input Fields")]
    [SerializeField] private CustomInputField playerNameField;
    [SerializeField] private CustomInputField playerNoteField;
    private void Awake()
    {
        scannerPanel = GetComponent<ScannerPanelManager>();
        foreach (var card in archetypeCards) card.onCardSwitched.AddListener(CardAdded);
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
    }
    private void CardAssignRequestToProfile(CardSO card)
    {
        if(!profilePanel.activeSelf) profilePanel.SetActive(true);
        GameManager.instance.LookingForCardToSelect(card);
    }
    private void CardAdded(int index)
    {
        cardAddedToProfile?.Invoke();
    }
}
