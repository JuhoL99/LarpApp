using UnityEngine;
using UnityEngine.Audio;


public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;


    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
    }
    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
    }
    public void SetFXVolume(float level)
    {
        audioMixer.SetFloat("fxVolume", Mathf.Log10(level) * 20f);
    }

    public void Start()
    {
        AudioSettings.Mobile.stopAudioOutputOnMute = true;
    }


}
