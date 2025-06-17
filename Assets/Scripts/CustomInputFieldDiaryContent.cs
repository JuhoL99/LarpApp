using TMPro;
using UnityEngine.Events;
using UnityEngine;

public class CustomInputFieldDiaryText : CustomInputField
{
    public UnityEvent<RectTransform> onFieldEditStarted = new UnityEvent<RectTransform>();

    private DiaryEntry linkedDiaryEntry;

    protected override void Awake()
    {
        base.Awake();
        inputField.contentType = TMP_InputField.ContentType.Standard;
        inputField.lineType = TMP_InputField.LineType.MultiLineNewline;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        if (inputField != null && linkedDiaryEntry != null)
        {
            inputField.text = linkedDiaryEntry.entryText;
        }
    }
    public override void EnableTextEdit()
    {
        onFieldEditStarted?.Invoke(GetComponent<RectTransform>());
        base.EnableTextEdit();
    }
    protected override void OnDisable()
    {
        if (inputField != null && linkedDiaryEntry != null)
        {
            linkedDiaryEntry.entryText = inputField.text;
        }
        base.OnDisable();
    }
    public override void StopTextEdit(string text = null)
    {
        //scrollRect.enabled = true;
        if (linkedDiaryEntry != null && text != null)
        {
            linkedDiaryEntry.entryText = text;
        }
        inputField.MoveTextStart(true);
        inputField.ForceLabelUpdate();
        base.StopTextEdit(text);
    }
    public override void LoadText()
    {
        if (linkedDiaryEntry != null && inputField != null)
        {
            inputField.text = linkedDiaryEntry.entryText;
        }
    }
}
