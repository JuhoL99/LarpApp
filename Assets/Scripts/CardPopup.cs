using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CardPopup : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button flipButton; //full
    [SerializeField] private Button rotateButton; //center on top
    [SerializeField] private Button qrButton;
    [SerializeField] private CardSO card;
    [SerializeField] private Transform popupBackground;

    [Header("QR Code Settings")]
    [SerializeField] private RectTransform qrCodeRect; // Assign the QR code's RectTransform
    [SerializeField] private float zoomScale = 3f; // How much to zoom
    [SerializeField] private float zoomDuration = 0.25f; // Animation duration

    [Header("Open card face or text on popup")]
    [SerializeField] private bool openCardTextSideOnPopup;

    private Image cardImg;
    private Image bgImg;
    private Sprite[] cardSides;
    private bool facingTop = true;
    private bool canRotate = true;

    // Store original values for reset
    private Vector3 originalScale;
    private Vector3 originalPosition;
    private bool isZoomed = false;

    private void Awake()
    {
        cardImg = gameObject.GetComponent<Image>();
        bgImg = popupBackground.GetComponent<Image>();
        cardSides = card.GetCardVisual();

        // Store original transform values
        originalScale = transform.localScale;
        originalPosition = transform.localPosition;

        if (flipButton != null) flipButton.onClick.AddListener(Flip);
        if (rotateButton != null) rotateButton.onClick.AddListener(Rotate);
        if (qrButton != null) qrButton.onClick.AddListener(ToggleQR);

        popupBackground.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        ResetTransforms();
        //PopupEffect();
    }

    private void ResetTransforms()
    {
        transform.localScale = Vector3.one;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.localPosition = originalPosition;

        if (!openCardTextSideOnPopup) facingTop = true;
        else facingTop = false;

        isZoomed = false;
        UpdateCardVisuals();
    }

    private void Rotate()
    {
        if (!canRotate || isZoomed) return; // Prevent rotation while zoomed
        canRotate = false;
        float rotationZ = transform.rotation.z == 0 ? -180 : 0;
        transform.DORotate(new Vector3(0, 0, rotationZ), 0.5f).OnComplete(() => canRotate = true);
    }

    public void PopupEffect(CardSO _card)
    {
        card = _card;
        UpdateCardVisuals();
        PopupEffect();
    }

    private void PopupEffect()
    {
        bgImg.color = new Color(0f, 0f, 0f, 0f);
        bgImg.DOColor(new Color(0f, 0f, 0f, 0.75f), 1f);
        transform.localScale = Vector3.zero;
        transform.DOScale(1f, 0.25f);
    }

    public void ClosePopup()
    {
        transform.parent.gameObject.SetActive(false);
        //transform.DOScale(0f, 1f);
    }

    public void Flip()
    {
        if (isZoomed) return; // Prevent flipping while zoomed

        if (facingTop) cardImg.sprite = cardSides[0];
        else cardImg.sprite = cardSides[1];
        transform.DOScaleX(0, 0.25f).onComplete = Flop;
    }

    private void Flop()
    {
        if (isZoomed) return;

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

        // Show/hide QR button based on card type
        if (qrButton != null)
            qrButton.gameObject.SetActive(card != null && card.markerType == MarkerType.Archetype);
    }

    public void InteractWithCard()
    {
        Flip();
    }

    private void ToggleQR()
    {
        if (!canRotate) return;

        if (!facingTop && !isZoomed)
        {
            ZoomToQR();
        }
        else if (isZoomed)
        {
            ZoomOut();
        }
    }

    // Zoom out of QR code
    private void ZoomOut()
    {
        isZoomed = false;

        // Animate back to original position and scale
        Sequence zoomOutSequence = DOTween.Sequence();
        zoomOutSequence.Append(transform.DOLocalMove(originalPosition, zoomDuration));
        zoomOutSequence.Join(transform.DOScale(originalScale, zoomDuration));
        zoomOutSequence.SetEase(Ease.OutCubic);
    }

    // Zoom to QR code
    private void ZoomToQR()
    {
        isZoomed = true;

        // Get QR code's position relative to the card
        Vector2 qrAnchoredPos = qrCodeRect.anchoredPosition;

        // Account for card rotation - if rotated 180 degrees, flip the QR position
        bool isRotated = Mathf.Abs(transform.rotation.eulerAngles.z - 180f) < 1f ||
                            Mathf.Abs(transform.rotation.eulerAngles.z + 180f) < 1f;

        if (isRotated)
        {
            qrAnchoredPos = -qrAnchoredPos; // Flip the position for 180-degree rotation
        }

        // Calculate how much to move the card to center the QR code
        Vector3 offsetToCenter = new Vector3(-qrAnchoredPos.x * zoomScale, -qrAnchoredPos.y * zoomScale, 0);
        Vector3 targetPosition = originalPosition + offsetToCenter;

        // Animate zoom
        Sequence zoomSequence = DOTween.Sequence();
        zoomSequence.Append(transform.DOLocalMove(targetPosition, zoomDuration));
        zoomSequence.Join(transform.DOScale(originalScale * zoomScale, zoomDuration));
        zoomSequence.SetEase(Ease.OutCubic);
    }
}