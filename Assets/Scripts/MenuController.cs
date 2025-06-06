using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using System.Collections.Generic;

public class MenuController : MonoBehaviour
{
    private bool isMenuOpen = false;

    [Header("Screen Settings")]
    [SerializeField] private GameObject mainScreen;
    [SerializeField] private List<GameObject> otherScreens = new List<GameObject>();
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject scannerPanel;
    [SerializeField] private GameObject backgroundImage;

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
        // Make sure start panel is always active first
        mainScreen.SetActive(false);
        ShowStartScreen();
    }

    void Update()
    {

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
        startPanel.SetActive(false);

        foreach (GameObject screen in otherScreens)
        {
            screen.SetActive(false);
        }
    }

    public void ShowStartScreen()
    {
        // Activate main screen and deactivate all others
        startPanel.SetActive(true);

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