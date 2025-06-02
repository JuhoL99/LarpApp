using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplayHandler : MonoBehaviour
{
    [Header("Connections")]
    [SerializeField] private Card card;
    [SerializeField] private CardSO cardSO;
    [SerializeField] private Button addCardButton;
    [SerializeField] private TMP_Text cardNameText;

    private void Start()
    {
        addCardButton.onClick.AddListener(() => GameManager.instance.scannerPanelManager.HandleCardManualAssignment(cardSO));
        UpdateDisplay();
    }
    public void InstantiatePrefab(CardSO _card)
    {
        card.SetCurrentCard(_card);
        card.canBeSwitched = false;
        cardSO = _card;
        UpdateDisplay();
    }
    private void UpdateDisplay()
    {
        cardNameText.text = card.GetCurrentCard().cardName;
    }
}
