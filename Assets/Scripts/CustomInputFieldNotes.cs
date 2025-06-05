using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomInputFieldNotes : CustomInputField
{
    //add to base class if als needed in name input field
    public UnityEvent<RectTransform> onFieldEditStarted = new UnityEvent<RectTransform>();

    protected override void Awake()
    {
        base.Awake();
        inputField.contentType = TMP_InputField.ContentType.Standard;
        inputField.lineType = TMP_InputField.LineType.MultiLineNewline;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        if (inputField != null && linkedUser != null)
        {
            inputField.text = linkedUser.userNotes;
        }
        if(linkedPlayer != null && inputField != null)
        {
            inputField.text = linkedPlayer.playerNotes;
        }
    }
    public override void EnableTextEdit()
    {
        onFieldEditStarted?.Invoke(GetComponent<RectTransform>());
        base.EnableTextEdit();
    }
    protected override void OnDisable()
    {
        if (inputField != null && linkedUser != null)
        {
            linkedUser.userNotes = inputField.text;
        }
        if (linkedPlayer != null && inputField != null)
        {
            linkedPlayer.playerNotes = inputField.text;
        }
        base.OnDisable();
    }
    public override void StopTextEdit(string text = null)
    {
        //scrollRect.enabled = true;
        if (linkedUser != null && text != null)
        {
            linkedUser.userNotes = text;
        }
        if(linkedPlayer != null && text != null)
        {
            linkedPlayer.playerNotes = text;
        }
        inputField.MoveTextStart(true);
        inputField.ForceLabelUpdate();
        base.StopTextEdit(text);
    }
    public override void LoadText()
    {
        if (linkedUser != null && inputField != null)
        {
            inputField.text = linkedUser.userNotes;
        }
        if(linkedPlayer != null && inputField != null)
        {
            inputField.text = linkedPlayer.playerNotes;
        }
    }
}
