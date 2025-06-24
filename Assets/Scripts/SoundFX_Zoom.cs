using UnityEngine;
using UnityEngine.UI;

public class SoundFX_Zoom : MonoBehaviour
{
    Button button;


    public void PlaySound()
    {
        // play sound fx from manager
        SoundFXManager.instance.PlayZoomSound();
        //print("Button clicked!");
    }

    private void Start()
    {
        button = GetComponent<Button>();

        // add onClick sound event to buttons
        button.onClick.RemoveListener(PlaySound);
        button.onClick.AddListener(PlaySound);
    }
}
