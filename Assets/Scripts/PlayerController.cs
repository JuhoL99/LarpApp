using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private InputSystem_Actions inputActions;
    private Vector2 moveInput;
    [SerializeField] private RectTransform test;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PhysicsMaterial2D mat = new PhysicsMaterial2D();
        mat.friction = 0.2f;
        rb.sharedMaterial = mat;
        inputActions = new InputSystem_Actions();
        inputActions.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Jump.performed += OnJump;
    }
    private void FixedUpdate()
    {
        rb.AddForce(moveInput * 800, ForceMode2D.Force);
        //test.Rotate(Vector3.forward, 10f);
    }
    private void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        Debug.Log(moveInput);
    }
    private void OnJump(InputAction.CallbackContext ctx)
    {
        rb.AddForce(Vector2.up * 1000, ForceMode2D.Impulse);
    }
}
