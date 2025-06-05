using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public abstract class CustomInputField : MonoBehaviour
{
    public Button editFieldButton;
    protected UserData linkedUser;
    protected PlayerData linkedPlayer;
    public TMP_InputField inputField;
    public Color editingColor;
    public Color normalColor;
    protected Image img;
    protected virtual void Awake()
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
    protected virtual void OnEnable()
    {
        img.color = normalColor;
        inputField.onDeselect.AddListener(StopTextEdit);
        editFieldButton.onClick.AddListener(EnableTextEdit);
    }
    public virtual void AssignPlayer(PlayerData player)
    {
        Debug.Log("assigned player");
        linkedPlayer = player;
        LoadText();
    }
    public virtual void AssignUser(UserData user)
    {
        linkedUser = user;
        LoadText();
    }
    protected virtual void OnDisable()
    {
        inputField.interactable = false;
        inputField.onDeselect.RemoveListener(StopTextEdit);
        editFieldButton.onClick.RemoveListener(EnableTextEdit);
    }
    protected virtual void Start()
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
        inputField.caretPosition = 0;
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
    protected virtual IEnumerator DisableInputNextFrame()
    {
        yield return null;
        inputField.interactable = false;
    }
}
