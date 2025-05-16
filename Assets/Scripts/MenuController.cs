using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using AllSamplesLauncher;
using System.Collections.Generic;

public class MenuController : MonoBehaviour
{
    public RectTransform menuPanel;
    public ScrollRect menuScrollRect;
    public GameObject scrollBlocker;
    public float slideSpeed = 10f;

    [Header("Swipe Settings")]
    public float swipeThreshold = 50f;
    public float edgeSwipeArea = 0.15f; // Percentage of screen width that counts as edge

    private bool isMenuOpen = false;
    private float menuWidth;
    private Vector2 startTouchPosition;
    private bool isDragging = false;

    [Header("Screen Settings")]
    [SerializeField] private GameObject mainScreen;
    [SerializeField] private List<GameObject> otherScreens = new List<GameObject>();
    [SerializeField] private GameObject headerPanel;
    [SerializeField] private GameObject sidePanel;

    // Input System action reference for back button
    private InputAction backAction;

    void Awake()
    {
        // Enable enhanced touch support
        EnhancedTouchSupport.Enable();

        // Set up back action using Input System
        backAction = new InputAction("Back");
        backAction.AddBinding("<Keyboard>/escape");
        backAction.performed += ctx => HandleBackButtonPress();
        backAction.Enable();
    }

    void Start()
    {
        // Ensure main screen is active at start
        ShowMainScreen();

        // Initialize swipe menu in closed position
        menuWidth = menuPanel.rect.width;
        menuPanel.anchoredPosition = new Vector2(-menuWidth, 0);

        scrollBlocker.SetActive(false);
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

    // Method to handle back button functionality
    private void HandleBackButtonPress()
    {
        // If a screen other than main is active, go back to main screen
        if (!IsMainScreenActive())
        {
            ShowMainScreen();
        }
        else
        {
            ExitMobileApp();
        }
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
                    scrollBlocker.SetActive(true);
                }
                // Left swipe to close
                else if (swipeDistance < -swipeThreshold && isMenuOpen)
                {
                    Debug.Log("Left swipe detected - Closing menu");
                    isMenuOpen = false;
                    scrollBlocker.SetActive(false);
                }

                isDragging = false;
            }
        }
    }

    public void ToggleSideMenu()
    {
        isMenuOpen = !isMenuOpen;
        scrollBlocker.SetActive(true);
    }

    public bool IsMainScreenActive()
    {
        // Check if main screen is active and all other screens are inactive
        if (!mainScreen.activeSelf)
            return false;

        foreach (GameObject screen in otherScreens)
        {
            if (screen.activeSelf)
                return false;
        }

        return true;
    }

    public void ShowMainScreen()
    {
        // Close menu if it's open when returning to main screen
        if (isMenuOpen)
        {
            isMenuOpen = false;
            scrollBlocker.SetActive(false);
        }

        // Activate main screen and deactivate all others
        mainScreen.SetActive(true);
        headerPanel.SetActive(true);
        sidePanel.SetActive(true);
        scrollBlocker.SetActive(false);

        foreach (GameObject screen in otherScreens)
        {
            screen.SetActive(false);
        }
    }

    // Public methods to be called from UI buttons
    public void ShowScreen(GameObject screen)
    {
        if (screen == mainScreen)
        {
            ShowMainScreen();
            return;
        }

        // Deactivate main screen and all other screens
        mainScreen.SetActive(false);

        foreach (GameObject otherScreen in otherScreens)
        {
            otherScreen.SetActive(otherScreen == screen);
        }
    }

    private void ExitMobileApp()
    {
        Debug.Log("Exiting to home screen...");
#if UNITY_EDITOR
        // In the editor, just stop play mode
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_ANDROID
        // Get the unity player activity
        AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        activity.Call<bool>("moveTaskToBack", true);
#elif UNITY_IOS
        // On iOS, there's no direct API to minimize
        // We can just print a debug message for now
        Debug.Log("iOS cannot directly minimize apps due to platform restrictions");
#else
        // On other platforms, fallback to Application.Quit()
        Application.Quit();
#endif
    }

    void OnDestroy()
    {
        // Clean up and disable input actions
        if (backAction != null)
        {
            backAction.Disable();
            backAction.Dispose();
        }

        // Disable enhanced touch support when the script is destroyed
        if (EnhancedTouchSupport.enabled)
            EnhancedTouchSupport.Disable();
    }
}