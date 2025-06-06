using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FontController : MonoBehaviour
{
    [Header("Font Size Settings")]
    private float newFontSize;
    [SerializeField] private bool includeInactive = true;

    [Header("Slider Settings")]
    [SerializeField] private Slider fontSizeSlider;
    [SerializeField] private float minFontSize = 72f;
    [SerializeField] private float maxFontSize = 100f;
    [SerializeField] private TextMeshProUGUI sliderValueText;

    [Header("Save System")]
    [SerializeField] private string saveKey = "FontSize";
    private float lastSavedValue;

    void Start()
    {
        LoadFontSize(); // Load saved font size on startup

        SetupSlider();
    }

    void SetupSlider()
    {
        if (fontSizeSlider != null)
        {
            // Configure slider range
            fontSizeSlider.minValue = minFontSize;
            fontSizeSlider.maxValue = maxFontSize;
            fontSizeSlider.value = newFontSize;

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

        SaveFontSize();
    }

    void UpdateSliderValueText()
    {
        if (sliderValueText != null)
        {
            sliderValueText.text = newFontSize.ToString("F0");
        }
    }

    public void ChangeAllFontSizes()
    {
        // Find all TextMeshPro components (UI)
        TextMeshProUGUI[] uiTexts = FindObjectsOfType<TextMeshProUGUI>(includeInactive);

        // Change UI TextMeshPro components
        foreach (TextMeshProUGUI text in uiTexts)
        {
            text.fontSize = newFontSize;
        }

        SaveFontSize();
    }

    public void SaveFontSize()
    {
        PlayerPrefs.SetFloat(saveKey, newFontSize);
        PlayerPrefs.Save();
        lastSavedValue = newFontSize;
        Debug.Log($"Font size {newFontSize} saved to key: {saveKey}");
    }

    public void LoadFontSize()
    {
        if (PlayerPrefs.HasKey(saveKey))
        {
            float savedValue = PlayerPrefs.GetFloat(saveKey, newFontSize);
            newFontSize = Mathf.Clamp(savedValue, minFontSize, maxFontSize);
            lastSavedValue = newFontSize;

            // Update slider if it exists
            if (fontSizeSlider != null)
            {
                fontSizeSlider.value = newFontSize;
            }

            UpdateSliderValueText();
            ChangeAllFontSizes();
            Debug.Log($"Font size {newFontSize} loaded from key: {saveKey}");
        }
    }

    // Method to reset all fonts to default if needed
    public void ChangeAllFontSizesToDefault(float defaultFontSize)
    {
        TextMeshProUGUI[] uiTexts = FindObjectsOfType<TextMeshProUGUI>(includeInactive);

        foreach (TextMeshProUGUI text in uiTexts)
        {
            text.fontSize = defaultFontSize;
        }

        SaveFontSize();
    }
    
    void OnDestroy()
    {
        // Final save before destruction if auto-save is enabled and value changed
        if (Mathf.Abs(newFontSize - lastSavedValue) > 0.1f)
        {
            SaveFontSize();
        }

        // Clean up slider listeners
        if (fontSizeSlider != null)
        {
            fontSizeSlider.onValueChanged.RemoveAllListeners();
        }
    }
}