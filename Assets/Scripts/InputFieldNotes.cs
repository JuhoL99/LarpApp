using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_InputField))]
public class InputFieldNotes : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button editTextButton;
    [Header("User Connected To Panel")]
    [SerializeField] private UserData linkedUser;
    [Header("Scroll")]
    [SerializeField] private ScrollRect scrollRect;
    private TMP_InputField inputField;

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        scrollRect = GetComponent<ScrollRect>();
        inputField.interactable = false;
        inputField.customCaretColor = true;
    }
    private void OnEnable()
    {
        if(linkedUser != null) inputField.text = linkedUser.userNotes;
        inputField.onDeselect.AddListener(StopTextEdit);
        editTextButton.onClick.AddListener(EnableTextEdit);
    }
    private void Start()
    {
        SaveLoadManager.instance.onGameLoaded.AddListener(LoadText);
    }
    private void OnDisable()
    {
        if(linkedUser != null) linkedUser.userNotes = inputField.text;
        inputField.interactable = false;
        inputField.onDeselect.RemoveListener(StopTextEdit);
        editTextButton.onClick.RemoveListener(EnableTextEdit);
    }
    private void EnableTextEdit()
    {
        if(inputField.interactable)
        {
            StopTextEdit();
            return;
        }
        inputField.interactable = true;
        ToggleCaret(true);
        inputField.Select();
        inputField.caretPosition = inputField.text.Length;
    }
    private void StopTextEdit(string text = null)
    {
        Debug.Log("this counts as end edit");
        if (linkedUser != null) linkedUser.userNotes = text;
        ToggleCaret(false);
        StartCoroutine(DisableInputNextFrame()); //otherwise you get annoying error in log
    }
    private void LoadText()
    {
        if(linkedUser != null) inputField.text = linkedUser.userNotes;
    }
    private IEnumerator DisableInputNextFrame()
    {
        yield return null;
        inputField.interactable = false;
    }
    public void AssignUserToNotes(UserData user)
    {
        linkedUser = user;
        LoadText();
    }
    private void ToggleCaret(bool toggle)
    {
        if(!toggle)
        {
            inputField.caretColor = new Color(0,0,0,0);
            return;
        }
        inputField.caretColor = new Color(0, 0, 0, 1);
    }
    private void TestFunction()
    {
        Debug.Log(linkedUser?.userNotes);
    }
}
