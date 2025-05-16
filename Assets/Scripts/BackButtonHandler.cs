using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BackButtonHandler : MonoBehaviour
{
    [SerializeField] private GameObject mainScreen;
    [SerializeField] private List<GameObject> otherScreens = new List<GameObject>();

    // Android back button action
    private InputAction backButtonAction;

    private void Awake()
    {
        // Set up a specific action for the Android back button
        backButtonAction = new InputAction("BackButton");

        // Map the back button correctly for Android
#if UNITY_ANDROID
        backButtonAction.AddBinding("<Keyboard>/escape");
#endif

        // Enable the action
        backButtonAction.Enable();

        // Subscribe to the action being performed
        backButtonAction.performed += ctx => HandleBackButtonPress();
        Input.backButtonLeavesApp = true;
    }

    private void OnDestroy()
    {
        // Clean up
        if (backButtonAction != null)
        {
            backButtonAction.performed -= ctx => HandleBackButtonPress();
            backButtonAction.Disable();
        }
    }

    private void Start()
    {
        // Ensure main screen is active at start
        ShowMainScreen();
    }

    private void Update()
    {

    }

    private void HandleBackButtonPress()
    {
        if (IsMainScreenActive())
        {
            MinimizeApp();
        }
        else
        {
            // Not at main screen, return to main screen
            ShowMainScreen();
        }
    }

    private bool IsMainScreenActive()
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

        foreach (GameObject screen in otherScreens)
        {
            screen.SetActive(false);
        }
    }

    private void MinimizeApp()
    {
        Debug.Log("Minimizing application");

#if UNITY_ANDROID
        // This is the proper way to minimize the app on Android
        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                activity.Call("moveTaskToBack", true);
            }
        }
#elif UNITY_EDITOR
        // In editor, just log that we would minimize
        Debug.Log("In a real device, the app would be minimized now");
#else
        // On other platforms, do nothing or implement platform-specific behavior
        Debug.Log("Minimize not implemented for this platform");
#endif
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
}