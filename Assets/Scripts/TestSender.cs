using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TestSender : MonoBehaviour
{
    public UnityEvent testEvent;
    [SerializeField] private InputAction action;
    private void Start()
    {
        action.performed += DoAction;
    }
    private void DoAction(InputAction.CallbackContext ctx)
    {
        testEvent.Invoke();
    }
}
