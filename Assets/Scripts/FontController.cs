using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextMeshProFontChanger : MonoBehaviour
{
    [Header("Font Size Settings")]
    [SerializeField] private float newFontSize = 72f;
    [SerializeField] private bool includeInactive = true;

    [Header("Slider Settings")]
    [SerializeField] private Slider fontSizeSlider;
    [SerializeField] private float minFontSize = 72f;
    [SerializeField] private float maxFontSize = 100f;
    [SerializeField] private TextMeshProUGUI sliderValueText; // Optional: Display current value

    [Header("Controls")]
    [SerializeField] private bool changeOnStart = false;

    void Start()
    {
        SetupSlider();

        if (changeOnStart)
        {
            ChangeAllFontSizes();
        }
    }

    void SetupSlider()
    {
        if (fontSizeSlider != null)
        {
            // Configure slider range
            fontSizeSlider.minValue = minFontSize;
            fontSizeSlider.maxValue = maxFontSize;
            fontSizeSlider.value = minFontSize;

            fontSizeSlider.onValueChanged.AddListener(OnSliderValueChanged);

            // Update slider value text if assigned
            UpdateSliderValueText();
        }
    }

    void OnSliderValueChanged(float value)
    {
        newFontSize = value;
        UpdateSliderValueText();
        ChangeAllFontSizes();
    }

    void UpdateSliderValueText()
    {
        if (sliderValueText != null)
        {
            sliderValueText.text = newFontSize.ToString("F0");
        }
    }

    [ContextMenu("Change All Font Sizes")]
    public void ChangeAllFontSizes()
    {
        // Find all TextMeshPro components (UI)
        TextMeshProUGUI[] uiTexts = FindObjectsOfType<TextMeshProUGUI>(includeInactive);

        int changedCount = 0;

        // Change UI TextMeshPro components
        foreach (TextMeshProUGUI text in uiTexts)
        {
            text.fontSize = newFontSize;
            changedCount++;
        }

        Debug.Log($"Changed font size to {newFontSize} for {changedCount} TextMeshPro components.");
    }

    [ContextMenu("Apply Slider Value")]
    public void ApplySliderValue()
    {
        if (fontSizeSlider != null)
        {
            newFontSize = fontSizeSlider.value;
            ChangeAllFontSizes();
        }
    }

    public void SetSliderValue(float value)
    {
        if (fontSizeSlider != null)
        {
            fontSizeSlider.value = Mathf.Clamp(value, minFontSize, maxFontSize);
        }
    }

    [ContextMenu("Reset All Font Sizes")]
    public void ResetAllFontSizes()
    {
        ChangeAllFontSizesToDefault(18f); // Default TMP font size
    }

    public void ChangeAllFontSizesToDefault(float defaultSize)
    {
        TextMeshProUGUI[] uiTexts = FindObjectsOfType<TextMeshProUGUI>(includeInactive);

        int changedCount = 0;

        foreach (TextMeshProUGUI text in uiTexts)
        {
            text.fontSize = defaultSize;
            changedCount++;
        }

        Debug.Log($"Reset font size to {defaultSize} for {changedCount} TextMeshPro components.");
    }

    // Method to change font sizes with specific parameters
    public void ChangeAllFontSizes(float fontSize, bool includeInactiveObjects = true)
    {
        TextMeshProUGUI[] uiTexts = FindObjectsOfType<TextMeshProUGUI>(includeInactiveObjects);

        int changedCount = 0;

        foreach (TextMeshProUGUI text in uiTexts)
        {
            text.fontSize = fontSize;
            changedCount++;
        }

        Debug.Log($"Changed font size to {fontSize} for {changedCount} TextMeshPro components.");
    }

    void OnDestroy()
    {
        // Clean up slider listeners
        if (fontSizeSlider != null)
        {
            fontSizeSlider.onValueChanged.RemoveAllListeners();
        }
    }
}