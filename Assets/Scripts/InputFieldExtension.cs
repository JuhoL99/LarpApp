using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class InputFieldExtension : MonoBehaviour
{
    private bool isKeyboardOpen;
    private bool open = false;
    private UnityEvent keyboardOpened = new UnityEvent();
    private UnityEvent keyboardClosed = new UnityEvent();
    private Vector2 originalPos;
    private Vector3 worldPos;

    private RectTransform currentEditedField;
    [SerializeField] private CustomInputFieldNotes[] fieldsEnabledOn;

    private void Start()
    {
        keyboardOpened.AddListener(OpenInputExtension);
        keyboardClosed.AddListener(CloseInputExtension);
        foreach(CustomInputFieldNotes field in fieldsEnabledOn)
        {
            field.onFieldEditStarted.AddListener(SetCurrentEditedField);
        }
    }
    private void Update()
    {
        CheckKeyboardState();
    }
    private void CheckKeyboardState()
    {
#if UNITY_ANDROID
        isKeyboardOpen = GameManager.instance.GetKeyboardSize() > 0;
        if(isKeyboardOpen && !open)
        {
            keyboardOpened?.Invoke();
        }
        else if(!isKeyboardOpen && open)
        {
            keyboardClosed?.Invoke();
        }
#elif UNITY_IOS
        isKeyboardOpen = TouchScreenKeyboard.area.height > 0;
        if (isKeyboardOpen && !open)
        {
            keyboardOpened?.Invoke();
        }
        else if (!isKeyboardOpen && open)
        {
            keyboardClosed?.Invoke();
        } 
#endif
    }
    private void OpenInputExtension()
    {
        open = true;
        currentEditedField.anchoredPosition =
            new Vector2(0f, GameManager.instance.GetKeyboardSize()
            - (worldPos.y - currentEditedField.rect.height/2));
    }
    private void CloseInputExtension()
    {
        open = false;
        currentEditedField.anchoredPosition = originalPos;
    }
    private void SetCurrentEditedField(RectTransform field)
    {
        currentEditedField = field;
        originalPos = field.anchoredPosition;
        worldPos = field.TransformPoint(field.pivot);
    }
}
