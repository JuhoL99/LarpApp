using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomInputFieldNotes : CustomInputField, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] protected TMP_Text dragText;
    [SerializeField] protected ScrollRect scrollRect;

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragText.text = $"drag start";
    }
    public void OnDrag(PointerEventData eventData)
    {
        dragText.text = $"dragging: {eventData.dragging}";
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        dragText.text = $"drag over";
    }
    public override void OnEnable()
    {
        base.OnEnable();
        if (inputField != null && linkedUser != null)
        {
            inputField.text = linkedUser.userNotes;
        }
    }
    public override void EnableTextEdit()
    {
        base.EnableTextEdit();
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
