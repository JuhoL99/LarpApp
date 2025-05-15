using UnityEngine;
using easyar;
using System.Collections;
using UnityEngine.UI;

public class ARCameraManager : MonoBehaviour
{
    private CameraDeviceFrameSource cameraDevice;
    public ARSession Session;

    private void Start()
    {
        // Disable camera at startup
        cameraDevice = Session.GetComponentInChildren<CameraDeviceFrameSource>();
        EnableCamera(false);
    }

    private void Update()
    {

    }

    public void EnableCamera(bool enable)
    {
        cameraDevice.enabled = enable;
    }
}