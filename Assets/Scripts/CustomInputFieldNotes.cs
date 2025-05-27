using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomInputFieldNotes : CustomInputField, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //[SerializeField] protected TMP_Text dragText;
    //[SerializeField] protected ScrollRect scrollRect;
    protected override void Awake()
    {
        base.Awake();
        inputField.contentType = TMP_InputField.ContentType.Standard;
        inputField.lineType = TMP_InputField.LineType.MultiLineNewline;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //dragText.text = $"drag start";
    }
    public void OnDrag(PointerEventData eventData)
    {
        //dragText.text = $"dragging: {eventData.dragging}";
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        //dragText.text = $"drag over";
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
        //scrollRect.enabled = false;
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
