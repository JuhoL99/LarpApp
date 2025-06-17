using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CustomInputFieldDiaryTitle : CustomInputField
{
    public UnityEvent<RectTransform> onFieldEditStarted = new UnityEvent<RectTransform>();

    private DiaryEntry linkedDiaryEntry;

    protected override void Awake()
    {
        base.Awake();
        inputField.contentType = TMPro.TMP_InputField.ContentType.Standard;
        inputField.characterLimit = 28;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        if (inputField != null && linkedDiaryEntry != null)
        {
            inputField.text = linkedDiaryEntry.entryTitle;
        }
    }
    protected override void OnDisable()
    {
        if (inputField != null && linkedDiaryEntry != null)
        {
            inputField.text = linkedDiaryEntry.entryTitle;
        }
        base.OnDisable();
    }
    public override void StopTextEdit(string text = null)
    {
        if (linkedDiaryEntry != null && text != null)
        {
            linkedDiaryEntry.entryTitle = text;
        }
        base.StopTextEdit(text);
    }
    public override void LoadText()
    {
        if (linkedDiaryEntry != null && inputField != null)
        {
            inputField.text = linkedDiaryEntry.entryTitle;
        }
    }
}
