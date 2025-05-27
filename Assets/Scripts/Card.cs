using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Card : MonoBehaviour
{
    [Header("Card base visuals")]
    [SerializeField] private Sprite[] sprites;
    [Header("Card order in hierarchy")]
    [SerializeField] private int cardIndex;
    private CardSO currentCard;
    private Image img;
    [Header("Buttons")]
    [SerializeField] private Button interactButton;
    private TMP_Text buttonText;
    private bool isEmpty;
    private bool facingTop = true;
    private bool canRotate = true;
    public UnityEvent<int> onCardSwitched;
    private void Start()
    {
        img = GetComponent<Image>();
        currentCard = GameManager.instance.cardDatabase.GetCardByID(-1);
        interactButton.onClick.AddListener(CardPopup);
        if(sprites != null) img.sprite = sprites[0];
    }
    private void AllowSwitch()
    {
        GameManager.instance.cardScanner.EnableScanning();
        GameManager.instance.cardScanner.onCardScanned.AddListener(GetScannedCard);
    }
    public void CancelSwitch()
    {
        GameManager.instance.cardScanner.DisableScanning();
        GameManager.instance.cardScanner.onCardScanned.RemoveListener(GetScannedCard);
    }
    private void GetScannedCard(CardSO card)
    {
        GameManager.instance.cardScanner.onCardScanned.RemoveListener(GetScannedCard);
        currentCard = card;
        onCardSwitched?.Invoke(cardIndex);
        UpdateCardVisuals();
    }
    public CardSO GetCurrentCard()
    {
        return currentCard;
    }
    public void SetCurrentCard(CardSO card)
    {
        if(img == null) img = GetComponent<Image>();
        currentCard = card;
        UpdateCardVisuals();
    }
    private void CheckCardSwitch()
    {
        if (GameManager.instance.isLookingForCardToSelect)
        {
            currentCard = GameManager.instance.currentScannedCard;
            onCardSwitched?.Invoke(cardIndex);
            UpdateCardVisuals();
            return;
        }
    }
    public void CardPopup()
    {
        GameObject popup = GameManager.instance.cardPopup;
        popup.SetActive(true);
        popup.GetComponentInChildren<CardAnimations>().PopupEffect(currentCard);
    }
    private void UpdateCardVisuals()
    {
        if (currentCard != null) sprites = currentCard.GetCardVisual();
        if (facingTop) img.sprite = sprites[0];
        else img.sprite = sprites[1];
    }
    public void InteractWithCard()
    {
        CardPopup();
    }
}
