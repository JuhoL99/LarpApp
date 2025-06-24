using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;
    private AudioSource audioSource;
    public AudioClip buttonSound;
    public AudioClip cardSound;
    public AudioClip zoomSound;
    public AudioClip boomSound;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
    }

    public void PlayButtonSound()
    {
        audioSource.PlayOneShot(buttonSound, 1f);
    }

    public void PlayCardSound()
    {
        audioSource.PlayOneShot(cardSound, 1f);
    }

    public void PlayZoomSound()
    {
        audioSource.PlayOneShot(zoomSound, 1f);
    }

    public void PlayBoomSound()
    {
        audioSource.PlayOneShot(boomSound, 1f);
    }


}
