using UnityEngine;

public class CustomInputFieldNotes : CustomInputField
{
    public override void OnEnable()
    {
        base.OnEnable();
        if (inputField != null && linkedUser != null)
        {
            inputField.text = linkedUser.userNotes;
        }
    }
    public override void OnDisable()
    {
        if (inputField != null && linkedUser != null)
        {
            linkedUser.userNotes = inputField.text;
        }
        base.OnDisable();
    }
    public override void StopTextEdit(string text = null)
    {
        if (linkedUser != null && text != null)
        {
            linkedUser.userNotes = text;
        }
        base.StopTextEdit(text);
    }
    public override void LoadText()
    {
        if (linkedUser != null && inputField != null)
        {
            inputField.text = linkedUser.userNotes;
        }
    }
}
