using TMPro;
using UnityEngine;

public class AnimateText : MonoBehaviour
{
    private TMP_Text textToAnimate;
    [Header("Animation Frequency (sec)")]
    [SerializeField] private float animationFrequency = 0.3f;
    private int animIndex = 0;
    [Header("Text sequence to play")]
    [SerializeField] private string[] textSequence;
    private void Start()
    {
        if(textSequence == null || textSequence.Length == 0) textSequence = 
                new string[4] { "SCANNING", "SCANNING .", "SCANNING . .", "SCANNING . . ." };
        textToAnimate = GetComponent<TMP_Text>();
        InvokeRepeating("Animate", 0f, animationFrequency);
    }
    private void Animate()
    {
        if(animIndex > textSequence.Length - 1) animIndex = 0;
        textToAnimate.text = textSequence[animIndex];
        animIndex++;
    }
}
