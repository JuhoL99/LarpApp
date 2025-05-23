using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class CustomInputField : MonoBehaviour
{
    public Button editFieldButton;
    public UserData linkedUser;
    public TMP_InputField inputField;
    public virtual void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.interactable = false;
        inputField.customCaretColor = true;
    }
    public virtual void OnEnable()
    {
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
        ToggleCaret(true);
        inputField.Select();
        inputField.caretPosition = inputField.text.Length;
    }
    public virtual void StopTextEdit(string text = null)
    {
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
