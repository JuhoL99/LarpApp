using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CardAnimations : MonoBehaviour
{
    [SerializeField] private Button flipButton;
    [SerializeField] private Button qrButton;
    [SerializeField] private CardSO card;
    [SerializeField] private Transform popupBackground;
    private Image cardImg;
    private Image bgImg;
    private Sprite[] cardSides;
    private bool isQRShowing = false;
    private bool facingTop = true;

    private void Awake()
    {
        cardImg = gameObject.GetComponent<Image>();
        bgImg = popupBackground.GetComponent<Image>();
        cardSides = card.GetCardVisual();
        if(flipButton != null) flipButton.onClick.AddListener(Flip);
        if(qrButton != null) qrButton.onClick.AddListener(ToggleQR);
        isQRShowing = false;
        popupBackground.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        PopupEffect();
    }
    private void PopupEffect()
    {
        bgImg.color = new Color(0f,0f, 0f, 0f);
        bgImg.DOColor(new Color(0f, 0f, 0f, 0.75f), 1f);
        transform.localScale = Vector3.zero;
        transform.DOScale(1f, 1f);
    }
    public void Flip()
    {
        if (facingTop) cardImg.sprite = cardSides[0];
        else cardImg.sprite = cardSides[1];
        transform.DOScaleX(0, 0.25f).onComplete = Flop;
    }
    private void Flop()
    {
        facingTop = !facingTop;
        if (facingTop) cardImg.sprite = cardSides[0];
        else cardImg.sprite = cardSides[1];
        transform.DOScaleX(1, 0.25f);
    }
    private void UpdateCardVisuals()
    {
        if (card != null) cardSides = card.GetCardVisual();
        if (facingTop) cardImg.sprite = cardSides[0];
        else cardImg.sprite = cardSides[1];
    }
    public void InteractWithCard()
    {
        Flip();
    }
    private void ToggleQR()
    {
        isQRShowing = !isQRShowing;
    }
}
