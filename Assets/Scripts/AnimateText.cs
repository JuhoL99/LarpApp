using TMPro;
using UnityEngine;

public class AnimateText : MonoBehaviour
{
    private TMP_Text textToAnimate;
    private float animationFrequency = 0.5f;
    private int animIndex = 0;
    private string[] textSequence;
    private void Start()
    {
        textToAnimate = GetComponent<TMP_Text>();
        textSequence = new string[4] {"SCANNING","SCANNING.","SCANNING..","SCANNING..."};
        InvokeRepeating("Animate", 0f, animationFrequency);
    }
    private void Animate()
    {
        if(animIndex > textSequence.Length - 1) animIndex = 0;
        textToAnimate.text = textSequence[animIndex];
        animIndex++;
    }
}
