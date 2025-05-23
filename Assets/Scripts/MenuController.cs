using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using System.Collections.Generic;

public class MenuController : MonoBehaviour
{
    // References to the side menu panel
    public RectTransform menuPanel;
    public ScrollRect menuScrollRect;
    public GameObject scrollBlocker;
    public float slideSpeed = 10f;

    [Header("Swipe Settings")]
    public float swipeThreshold = 50f;
    public float edgeSwipeArea = 0.15f; // Percentage of screen width that counts as edge
    public float snapThreshold = 0.5f;  // Threshold for snapping (0.5 = 50% of menu width)

    private bool isMenuOpen = false;
    private float menuWidth;
    private Vector2 startTouchPosition;
    private bool isDragging = false;
    private bool isSwipingFromEdge = false;
    private float initialMenuPosition;

    [Header("Screen Settings")]
    [SerializeField] private GameObject mainScreen;
    [SerializeField] private List<GameObject> otherScreens = new List<GameObject>();
    [SerializeField] private GameObject headerPanel;
    [SerializeField] private GameObject sidePanel;
    [SerializeField] private GameObject scannerPanel;

    // Input System action reference for back button
    private InputAction backAction;

    public ARCameraManager arCameraManager;

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
        menuWidth = menuPanel.rect.width / 2;
        menuPanel.anchoredPosition = new Vector2(-menuWidth, 0);

        // Disable the scroll blocker on start 
        scrollBlocker.SetActive(false);

        arCameraManager = FindAnyObjectByType<ARCameraManager>();
    }

    void Update()
    {
        // Handle menu animation when not dragging
        if (!isDragging)
        {
            AnimateMenuToTargetPosition();
        }

        // Process touch input for dragging
        HandleTouchInput();

        // Set scroll blocker active when needed
        SetScrollBlocker();

        // Set camera active when needed
        SetCamera();
    }

    private void HandleTouchInput()
    {
        if (Touch.activeTouches.Count > 0)
        {
            Touch touch = Touch.activeTouches[0];
            float edgeWidth = Screen.width * edgeSwipeArea;

            // Touch began
            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.screenPosition;
                initialMenuPosition = menuPanel.anchoredPosition.x;

                // Check if touch started from edge or if menu is already partly open
                isSwipingFromEdge = startTouchPosition.x < edgeWidth || (isMenuOpen && menuPanel.anchoredPosition.x > -menuWidth);

                if (isSwipingFromEdge)
                {
                    isDragging = true;
                }
            }
            // Touch moved
            else if (touch.phase == TouchPhase.Moved && isDragging && isSwipingFromEdge)
            {
                // Calculate drag distance
                float dragDistance = touch.screenPosition.x - startTouchPosition.x;

                // Apply drag to menu position
                float newX = Mathf.Clamp(initialMenuPosition + dragDistance, -menuWidth, 0);
                menuPanel.anchoredPosition = new Vector2(newX, 0);
            }
            // Touch ended
            else if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && isDragging)
            {
                // Determine if menu should snap open or closed based on position
                float menuOpenPercentage = (menuPanel.anchoredPosition.x + menuWidth) / menuWidth;
                isMenuOpen = menuOpenPercentage > snapThreshold;

                isDragging = false;
                isSwipingFromEdge = false;
            }
        }
    }

    private void AnimateMenuToTargetPosition()
    {
        float targetX = isMenuOpen ? 0 : -menuWidth;
        menuPanel.anchoredPosition = Vector2.Lerp(menuPanel.anchoredPosition, new Vector2(targetX, 0), Time.deltaTime * slideSpeed);
    }

    // Method to handle back button functionality
    private void HandleBackButtonPress()
    {
        // If the side menu is open, close it
        if (isMenuOpen)
        {
            ToggleSideMenu();
        }
        else
        {
            // If a screen other than main is active, go back to main screen
            // If main screen is active, minimize the app
            if (!IsMainScreenActive())
            {
                ShowMainScreen();
            }
            else
            {
                ExitMobileApp();
            }
        }
    }

    public void ToggleSideMenu()
    {
        isMenuOpen = !isMenuOpen;
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
        // Activate main screen and deactivate all others
        mainScreen.SetActive(true);
        headerPanel.SetActive(true);
        sidePanel.SetActive(true);

        foreach (GameObject screen in otherScreens)
        {
            screen.SetActive(false);
        }
    }

    // Public methods to be called from UI buttons
    public void ShowScreen(GameObject screen)
    {
        // Close the menu directly instead of toggling
        isMenuOpen = false;
        isDragging = false;

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

    void SetCamera()
    {
        if (scannerPanel.activeSelf)
        {
            arCameraManager.EnableCamera(true);
        }
        else
        {
            arCameraManager.EnableCamera(false);
        }
    }

    void SetScrollBlocker()
    {
        // If main screen is active and the menu is open, enable scroll blocker
        if ((isMenuOpen) || (isDragging))
        {
            scrollBlocker.SetActive(true);
        }
        else
        {
            scrollBlocker.SetActive(false);
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