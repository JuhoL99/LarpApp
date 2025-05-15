using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class MenuController : MonoBehaviour
{
    public RectTransform menuPanel;
    public ScrollRect menuScrollRect;
    public float slideSpeed = 10f;
    public Button menuButton;

    [Header("Swipe Settings")]
    public float swipeThreshold = 50f;
    public float edgeSwipeArea = 0.15f; // Percentage of screen width that counts as edge

    private bool isMenuOpen = false;
    private float menuWidth;
    private Vector2 startTouchPosition;
    private bool isDragging = false;

    void Awake()
    {
        // Enable enhanced touch support
        EnhancedTouchSupport.Enable();
    }

    void Start()
    {
        // Initialize menu in closed position
        menuWidth = menuPanel.rect.width;
        menuPanel.anchoredPosition = new Vector2(-menuWidth, 0);

        Debug.Log("SlideMenu initialized. Menu width: " + menuWidth);
    }

    void Update()
    {
        // Handle menu animation
        float targetX = isMenuOpen ? 0 : -menuWidth;
        menuPanel.anchoredPosition = Vector2.Lerp(
            menuPanel.anchoredPosition,
            new Vector2(targetX, 0),
            Time.deltaTime * slideSpeed
        );

        // Process swipe detection
        DetectSwipe();
    }

    void DetectSwipe()
    {
        // Handle touch input on mobile using EnhancedTouch
        if (Touch.activeTouches.Count > 0)
        {
            Touch touch = Touch.activeTouches[0];
            float edgeWidth = Screen.width * edgeSwipeArea;

            // Touch began
            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.screenPosition;
                isDragging = true;
                Debug.Log("Touch began at: " + startTouchPosition);
            }
            // Touch ended
            else if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && isDragging)
            {
                float swipeDistance = touch.screenPosition.x - startTouchPosition.x;

                Debug.Log("Touch ended. Swipe distance: " + swipeDistance + ", Started at X: " + startTouchPosition.x + ", Edge width: " + edgeWidth);

                // Right swipe from left edge to open
                if (swipeDistance > swipeThreshold && startTouchPosition.x < edgeWidth)
                {
                    Debug.Log("Right edge swipe detected - Opening menu");
                    isMenuOpen = true;
                    menuScrollRect.enabled = false;
                }
                // Left swipe to close
                else if (swipeDistance < -swipeThreshold && isMenuOpen)
                {
                    Debug.Log("Left swipe detected - Closing menu");
                    isMenuOpen = false;
                    menuScrollRect.enabled = true;
                }

                isDragging = false;
            }
        }
    }

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        menuScrollRect.enabled = false;
    }

    void OnDestroy()
    {
        // Disable enhanced touch support when the script is destroyed
        if (EnhancedTouchSupport.enabled)
            EnhancedTouchSupport.Disable();
    }
}