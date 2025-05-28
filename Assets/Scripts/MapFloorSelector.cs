using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ImageDropdown : MonoBehaviour
{
    [Header("Main Button")]
    public Button mainButton;
    public Image mainButtonImage;

    [Header("Dropdown")]
    public GameObject dropdownPanel;
    public CanvasGroup dropdownCanvasGroup;

    [Header("Options")]
    public List<DropdownOption> options = new List<DropdownOption>();

    [Header("Layout Settings")]
    public float buttonSpacing = 175f;
    public bool autoArrangeOnStart = true;

    [Header("Animation Settings")]
    public float animationDuration = 0.2f;
    public AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private bool isDropdownOpen = false;
    private int selectedIndex = 0;

    [System.Serializable]
    public class DropdownOption
    {
        public Sprite sprite;
        public Button button;
        public Image buttonImage;
    }

    void Start()
    {
        InitializeDropdown();
    }

    void InitializeDropdown()
    {
        // Set initial state
        dropdownPanel.SetActive(false);

        // Arrange buttons if enabled
        if (autoArrangeOnStart)
        {
            ArrangeButtons();
        }

        // Setup main button
        mainButton.onClick.AddListener(ToggleDropdown);

        // Setup option buttons
        for (int i = 0; i < options.Count; i++)
        {
            int index = i; // Capture for closure

            if (options[i].button != null)
            {
                // Set the image for the option button
                if (options[i].buttonImage != null && options[i].sprite != null)
                {
                    options[i].buttonImage.sprite = options[i].sprite;
                }

                // Add click listener
                options[i].button.onClick.AddListener(() => SelectOption(index));
            }
        }
    }

    public void ToggleDropdown()
    {
        if (isDropdownOpen)
        {
            CloseDropdown();
        }
        else
        {
            OpenDropdown();
        }
    }

    public void OpenDropdown()
    {
        if (isDropdownOpen) return;

        isDropdownOpen = true;
        dropdownPanel.SetActive(true);

        // Animate dropdown opening
        StartCoroutine(AnimateDropdown(0f, 1f));
    }

    public void CloseDropdown()
    {
        if (!isDropdownOpen) return;

        isDropdownOpen = false;

        // Animate dropdown closing
        StartCoroutine(AnimateDropdown(1f, 0f, () => {
            dropdownPanel.SetActive(false);
        }));
    }

    public void SelectOption(int index)
    {
        if (index < 0 || index >= options.Count) return;

        selectedIndex = index;

        // Update main button image
        if (options[index].sprite != null)
        {
            mainButtonImage.sprite = options[index].sprite;
        }

        // Close dropdown
        CloseDropdown();
    }

    private IEnumerator AnimateDropdown(float from, float to, System.Action onComplete = null)
    {
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animationDuration;
            float alpha = Mathf.Lerp(from, to, animationCurve.Evaluate(t));

            dropdownCanvasGroup.alpha = alpha;

            yield return null;
        }

        dropdownCanvasGroup.alpha = to;
        onComplete?.Invoke();
    }

    // Automatically arranges all option buttons vertically with consistent spacing
    public void ArrangeButtons()
    {
        if (options == null || options.Count == 0) return;

        for (int i = 0; i < options.Count; i++)
        {
            if (options[i].button != null)
            {
                RectTransform buttonRect = options[i].button.GetComponent<RectTransform>();
                if (buttonRect != null)
                {
                    // Set position - first button starts at -buttonSpacing, then each subsequent button moves down by buttonSpacing
                    Vector3 currentPos = buttonRect.anchoredPosition;
                    buttonRect.anchoredPosition = new Vector2(currentPos.x, -(i + 1) * buttonSpacing);
                }
            }
        }
    }
}