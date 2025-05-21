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
    private TMP_InputField inputField;
    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.interactable = false;
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
        inputField.Select();
    }
    private void StopTextEdit(string text = null)
    {
        Debug.Log("this counts as end edit");
        if (linkedUser != null) linkedUser.userNotes = text;
        StartCoroutine(DisableInputNextFrame()); //otherwise you get annoying error in log
    }
    private void LoadText()
    {
        if(linkedUser != null) inputField.text = linkedUser.userNotes;
        TestFunction();
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
    private void TestFunction()
    {
        Debug.Log(linkedUser?.userNotes);
    }
}
