using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class CustomInputField : MonoBehaviour
{
    public Button editFieldButton;
    protected UserData linkedUser;
    protected TMP_InputField inputField;
    public Color editingColor;
    public Color normalColor;
    protected Image img;
    public virtual void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        img = GetComponent<Image>();
        img.color = normalColor;
        inputField.interactable = false;
        inputField.customCaretColor = true;
        inputField.onFocusSelectAll = false;
        inputField.resetOnDeActivation = false;
        inputField.shouldHideMobileInput = true;
        inputField.restoreOriginalTextOnEscape = false;
    }
    public virtual void OnEnable()
    {
        img.color = normalColor;
        inputField.onDeselect.AddListener(StopTextEdit);
        editFieldButton.onClick.AddListener(EnableTextEdit);
    }
    public virtual void AssignUser(UserData user)
    {
        linkedUser = user;
        LoadText();
    }
    public virtual void OnDisable()
    {
        inputField.interactable = false;
        inputField.onDeselect.RemoveListener(StopTextEdit);
        editFieldButton.onClick.RemoveListener(EnableTextEdit);
    }
    public virtual void Start()
    {
        SaveLoadManager.instance.onGameLoaded.AddListener(LoadText);
    }
    public virtual void EnableTextEdit()
    {
        if (inputField.interactable)
        {
            StopTextEdit();
            return;
        }
        inputField.interactable = true;
        img.color = editingColor;
        ToggleCaret(true);
        inputField.Select();
        inputField.caretPosition = inputField.text.Length;
    }
    public virtual void StopTextEdit(string text = null)
    {
        img.color = normalColor;
        ToggleCaret(false);
        StartCoroutine(DisableInputNextFrame());
    }
    public virtual void LoadText()
    {

    }
    public virtual void ToggleCaret(bool toggle)
    {
        if (!toggle)
        {
            inputField.caretColor = new Color(0, 0, 0, 0);
            return;
        }
        inputField.caretColor = new Color(0, 0, 0, 1);
    }
    public virtual IEnumerator DisableInputNextFrame()
    {
        yield return null;
        inputField.interactable = false;
    }
}
