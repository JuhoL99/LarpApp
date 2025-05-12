using DG.Tweening;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private CardScanner scanner;
    private CardSO currentCard;
    private Image img;
    private Button switchCardButton;
    private bool isEmpty;
    private bool facingTop = true;

    private void Start()
    {
        img = GetComponent<Image>();
        currentCard = GameManager.instance.cardDatabase.GetCardByID(0);
        switchCardButton = transform.parent.GetChild(0).GetComponent<Button>();
        switchCardButton.onClick.AddListener(AllowSwitch);
        if(sprites != null) img.sprite = sprites[0];
    }
    private void AllowSwitch()
    {
        scanner.EnableScanning();
        scanner.onCardScanned.AddListener(GetScannedCard);
    }
    public void CancelSwitch()
    {
        scanner.DisableScanning();
        scanner.onCardScanned.RemoveListener(GetScannedCard);
    }
    private void GetScannedCard(CardSO card)
    {
        scanner.onCardScanned.RemoveListener(GetScannedCard);
        currentCard = card;
        UpdateCardVisuals();
    }
    public void Flip()
    {
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
