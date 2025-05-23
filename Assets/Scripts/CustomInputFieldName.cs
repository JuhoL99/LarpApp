using UnityEngine;

public class CustomInputFieldName : CustomInputField
{
    public override void OnEnable()
    {
        base.OnEnable();
        if (inputField != null && linkedUser != null)
        {
            inputField.text = linkedUser.userName;
        }
    }
    public override void OnDisable()
    {
        if (inputField != null && linkedUser != null)
        {
            linkedUser.userName = inputField.text;
        }
        base.OnDisable();
    }
    public override void StopTextEdit(string text = null)
    {
        if (linkedUser != null && text != null)
        {
            linkedUser.userName = text;
        }
        base.StopTextEdit(text);
    }
    public override void LoadText()
    {
        if (linkedUser != null && inputField != null)
        {
            inputField.text = linkedUser.userName;
        }
    }
}
