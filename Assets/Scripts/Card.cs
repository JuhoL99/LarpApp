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
    private Button switchCardButton;
    [Header("Buttons")]
    [SerializeField] private Button flipButton; //full
    [SerializeField] private Button rotateButton; //center on top
    private TMP_Text buttonText;
    private bool isEmpty;
    private bool facingTop = true;
    private bool canRotate = true;
    public UnityEvent<int> onCardSwitched;
    private void Start()
    {
        img = GetComponent<Image>();
        currentCard = GameManager.instance.cardDatabase.GetCardByID(-1);
        if(flipButton != null) flipButton.onClick.AddListener(CheckCardSwitch);
        if (flipButton != null) flipButton.onClick.AddListener(Flip);
        if (rotateButton != null) rotateButton.onClick.AddListener(CheckCardSwitch);
        if (rotateButton != null) rotateButton.onClick.AddListener(Rotate);
        //switchCardButton = transform.parent.GetChild(0).GetComponent<Button>();
        //buttonText = switchCardButton.transform.GetComponentInChildren<TMP_Text>();
        //buttonText.text = "Scan Card";
        //switchCardButton.onClick.AddListener(AllowSwitch);
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
    public void Rotate()
    {
        return;
        if (!canRotate) return;
        canRotate = false;
        float rotationZ = transform.rotation.z == 0 ? -180 : 0;
        transform.DORotate(new Vector3(0, 0, rotationZ), 0.5f).OnComplete(() => canRotate = true);
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
    public void Flip()
    {
        //move elsewhere >
        GameObject popup = GameManager.instance.cardPopup;
        popup.SetActive(true);
        popup.GetComponentInChildren<CardAnimations>().PopupEffect(currentCard);

        return;
        if (facingTop) img.sprite = sprites[0];
        else img.sprite = sprites[1];
        transform.DOScaleX(0, 0.25f).onComplete = Flop;
    }
    private void Flop()
    {
        facingTop = !facingTop;
        if (facingTop) img.sprite = sprites[0];
        else img.sprite = sprites[1];
        transform.DOScaleX(1, 0.25f);
    }
    private void UpdateCardVisuals()
    {
        if (currentCard != null) sprites = currentCard.GetCardVisual();
        if (facingTop) img.sprite = sprites[0];
        else img.sprite = sprites[1];
    }
    public void InteractWithCard()
    {
        Flip();
    }
}
