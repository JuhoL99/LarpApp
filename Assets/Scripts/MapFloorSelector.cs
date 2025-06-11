using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class MapFloorSelector : MonoBehaviour
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

    [Header("Animation Settings")]
    public float animationDuration = 0.2f;
    public AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private bool isDropdownOpen = false;
    public int selectedIndex = 0;

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

        // Setup main button
        mainButton.onClick.AddListener(ToggleDropdown);

        // Setup option buttons
        for (int i = 0; i < options.Count; i++)
        {
            int index = i; // Capture for closure

            if (options[i].button != null)
            {
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
}