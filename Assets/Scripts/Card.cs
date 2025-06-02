using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using easyar;
using Image = UnityEngine.UI.Image;
using Unity.VisualScripting;

public class Card : MonoBehaviour
{
    [Header("Card Type: Archetype - Faith")]
    [SerializeField] private MarkerType cardSlotType;
    [Header("Card base visuals")]
    [SerializeField] private Sprite[] sprites;
    [Header("Card order in hierarchy")]
    [SerializeField] private int cardIndex;
    [SerializeField] private CardSO currentCard;
    private Image img;
    [Header("Buttons")]
    [SerializeField] private Button interactButton;
    private TMP_Text buttonText;
    private bool isEmpty;
    private bool facingTop = true;
    private bool canRotate = true;
    public bool canBeSwitched = true;
    public UnityEvent<int, MarkerType> onCardSwitched;
    private void Start()
    {
        img = GetComponent<Image>();
        //currentCard = GameManager.instance.cardDatabase.GetCardByID(-1);
        interactButton.onClick.AddListener(CardPopup);
        if(sprites != null) UpdateCardVisuals();
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
        onCardSwitched?.Invoke(cardIndex, card.markerType);
        UpdateCardVisuals();
    }
    public CardSO GetCurrentCard()
    {
        return currentCard;
    }
    public void SetCurrentCard(CardSO card)
    {
        if(card == null)
        {
            Debug.Log("card null");
            return;
        }
        if(img == null) img = GetComponent<Image>();
        currentCard = card;
        //show fate card back since top is same for all
        if (card.markerType == MarkerType.Fate) facingTop = false;
        UpdateCardVisuals();
    }
    private void CheckCardSwitch()
    {
        if(!canBeSwitched) return;
        if (GameManager.instance.isLookingForCardToSelect)
        {
            if (GameManager.instance.currentScannedCard.markerType != cardSlotType) return;
            currentCard = GameManager.instance.currentScannedCard;
            onCardSwitched?.Invoke(cardIndex, currentCard.markerType);
            UpdateCardVisuals();
            return;
        }
    }
    public void CardPopup()
    {
        //move elsewhere>>
        if (currentCard == null) currentCard = GameManager.instance.cardDatabase.GetCardByID(-1);
        CheckCardSwitch();
        //<<
        if (currentCard.cardId < 0) return; //cant interact with default card ids -1 && -2
        GameObject popup = GameManager.instance.cardPopup;
        popup.SetActive(true);
        popup.GetComponentInChildren<CardPopup>().PopupEffect(currentCard);
    }
    public void UpdateCardVisuals()
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
