using UnityEngine;

public class CustomInputFieldName : CustomInputField
{
    protected override void Awake()
    {
        base.Awake();
        inputField.contentType = TMPro.TMP_InputField.ContentType.Standard;
        inputField.characterLimit = 28;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        if (inputField != null && linkedUser != null)
        {
            inputField.text = linkedUser.userName;
        }
        if(linkedPlayer != null && inputField != null)
        {
            inputField.text = linkedPlayer.playerName;
        }
    }
    protected override void OnDisable()
    {
        if (inputField != null && linkedUser != null)
        {
            linkedUser.userName = inputField.text;
        }
        if(linkedPlayer != null)
        {
            linkedPlayer.playerName = inputField.text;
        }
        base.OnDisable();
    }
    public override void StopTextEdit(string text = null)
    {
        if (linkedUser != null && text != null)
        {
            linkedUser.userName = text;
        }
        if(linkedPlayer != null && text != null)
        {
            linkedPlayer.playerName = text;
        }
        base.StopTextEdit(text);
    }
    public override void LoadText()
    {
        if (linkedUser != null && inputField != null)
        {
            inputField.text = linkedUser.userName;
        }
        if(linkedPlayer != null && inputField != null)
        {
            inputField.text = linkedPlayer.playerName;
        }
    }
}
