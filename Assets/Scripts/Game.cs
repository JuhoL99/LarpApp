using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField] private InputAction rotate;
    [SerializeField] private InputAction move;
    [SerializeField] private InputAction fire;
    [SerializeField] private Transform front;

    private float rotationDirection;
    private Vector2 moveDirection;

    private void Start()
    {
        SetupControls();
        front = transform.GetChild(0);
    }
    private void SetupControls()
    {
        rotate.Enable();
        move.Enable();
        fire.Enable();
        rotate.performed += OnRotate;
        rotate.canceled += OnRotate;
        move.performed += OnMove;
        move.canceled += OnMove;
        fire.performed += OnFire;
    }
    private void Update()
    {
        transform.Rotate(new Vector3(0,0, -rotationDirection * 100f * Time.deltaTime));
        transform.Translate(moveDirection * 100f * Time.deltaTime);
    }
    private void OnRotate(InputAction.CallbackContext ctx)
    {
        if(ctx.performed) rotationDirection = ctx.ReadValue<float>();
        else if(ctx.canceled) rotationDirection = ctx.ReadValue<float>();

    }
    private void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) moveDirection = ctx.ReadValue<Vector2>();
        else if (ctx.canceled) moveDirection = Vector2.zero;
    }
    private void OnFire(InputAction.CallbackContext ctx)
    {
        Fire();
    }
    private void Fire()
    { 
        GameObject go = new GameObject("bullet");
        go.transform.parent = transform.parent;
        go.transform.position = front.transform.position;
        go.transform.rotation = transform.rotation;
        go.transform.localScale = Vector3.one * 0.1f;
        Image img = go.AddComponent<Image>();
        img.color = Color.yellow;
        StartCoroutine(MoveBullet(go));
    }
    private IEnumerator MoveBullet(GameObject bullet)
    {
        float time = 5f;
        float timer = 0;
        Vector2 dir = transform.up;
        while (timer < time)
        {
            bullet.transform.position += (Vector3)(dir * Time.deltaTime * 200f);
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(bullet);
    }
}
