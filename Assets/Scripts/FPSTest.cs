using UnityEngine;

public class FPSTest : MonoBehaviour
{

    void Awake()
    {
        // Get device refresh rate
        int refreshRate = Screen.currentResolution.refreshRate;

        // Set target frame rate to device refresh rate
        // If we can't detect it, default to 60
        Application.targetFrameRate = refreshRate > 0 ? refreshRate : 60;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
