using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MapTouchController : MonoBehaviour
{
    [Header("Map Settings")]
    public RectTransform mapTransform; // The UI Image RectTransform
    public float minZoom = 0.5f;
    public float maxZoom = 3f;
    public float zoomSpeed = 1f;
    public float rotationSpeed = 1f;

    [Header("Pan Settings")]
    private bool enablePanning = true;
    public float panSpeed = 2f;

    [Header("Gesture Detection")]
    public float movementThreshold = 20f; // Minimum movement to start detecting gestures
    public float gestureDeadZone = 15f; // Dead zone angle (degrees) where no gesture is detected

    private Vector2 lastTouchPosition;
    private float lastTouchDistance;
    private float lastTouchAngle;
    private bool isPanning = false;
    private Vector3 initialScale;
    private float initialRotation;

    // Gesture detection variables
    private enum GestureType { None, Zoom, Rotation }
    private GestureType currentGesture = GestureType.None;
    private Vector2 touch1StartPosition;
    private Vector2 touch2StartPosition;
    private float totalMovementDistance = 0f;

    // Input System references
    private Touchscreen touchscreen;

    void Start()
    {
        if (mapTransform == null)
            mapTransform = GetComponent<RectTransform>();

        initialScale = mapTransform.localScale;
        initialRotation = mapTransform.eulerAngles.z;

        // Get input devices
        touchscreen = Touchscreen.current;
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        // Handle touch input
        if (touchscreen != null)
        {
            var touches = touchscreen.touches;
            int activeTouchCount = 0;

            // Count active touches
            for (int i = 0; i < touches.Count; i++)
            {
                if (touches[i].isInProgress)
                    activeTouchCount++;
            }

            if (activeTouchCount == 1)
            {
                HandleSingleTouch();
            }
            else if (activeTouchCount == 2)
            {
                HandleTwoFingerGestures();
            }
            else
            {
                isPanning = false;
            }
        }
    }

    void HandleSingleTouch()
    {
        if (touchscreen == null) return;

        var touch = GetFirstActiveTouch();
        if (touch == null) return;

        Vector2 touchPosition = touch.position.ReadValue();
        var phase = touch.phase.ReadValue();

        if (enablePanning)
        {
            if (phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                isPanning = true;
                lastTouchPosition = touchPosition;
            }
            else if (phase == UnityEngine.InputSystem.TouchPhase.Moved && isPanning)
            {
                Vector2 deltaPosition = (touchPosition - lastTouchPosition) * panSpeed;
                mapTransform.anchoredPosition += deltaPosition;
                lastTouchPosition = touchPosition;
            }
            else if (phase == UnityEngine.InputSystem.TouchPhase.Ended ||
                     phase == UnityEngine.InputSystem.TouchPhase.Canceled)
            {
                isPanning = false;
            }
        }
    }

    void HandleTwoFingerGestures()
    {
        if (touchscreen == null) return;

        var activeTouches = GetActiveTouches();
        if (activeTouches.Count < 2) return;

        var touch1 = activeTouches[0];
        var touch2 = activeTouches[1];

        Vector2 touch1Position = touch1.position.ReadValue();
        Vector2 touch2Position = touch2.position.ReadValue();

        var touch1Phase = touch1.phase.ReadValue();
        var touch2Phase = touch2.phase.ReadValue();

        // Calculate current distance and angle between touches
        float currentDistance = Vector2.Distance(touch1Position, touch2Position);
        Vector2 direction = touch2Position - touch1Position;
        float currentAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (touch1Phase == UnityEngine.InputSystem.TouchPhase.Began ||
            touch2Phase == UnityEngine.InputSystem.TouchPhase.Began)
        {
            // Initialize tracking values
            lastTouchDistance = currentDistance;
            lastTouchAngle = currentAngle;
            touch1StartPosition = touch1Position;
            touch2StartPosition = touch2Position;
            currentGesture = GestureType.None;
            totalMovementDistance = 0f;
            isPanning = false;
        }
        else if (touch1Phase == UnityEngine.InputSystem.TouchPhase.Moved ||
                 touch2Phase == UnityEngine.InputSystem.TouchPhase.Moved)
        {
            // Calculate movement from start positions
            Vector2 touch1Movement = touch1Position - touch1StartPosition;
            Vector2 touch2Movement = touch2Position - touch2StartPosition;
            float touch1MovementMagnitude = touch1Movement.magnitude;
            float touch2MovementMagnitude = touch2Movement.magnitude;

            // Track total movement distance
            totalMovementDistance += (touch1MovementMagnitude + touch2MovementMagnitude) * 0.5f;

            // Determine gesture type if not already determined and enough movement has occurred
            if (currentGesture == GestureType.None && totalMovementDistance > movementThreshold)
            {
                currentGesture = DetermineGestureFromMovement(touch1Movement, touch2Movement);
            }

            // Execute the appropriate gesture
            if (currentGesture == GestureType.Zoom)
            {
                HandleZoomGesture(currentDistance);
            }
            else if (currentGesture == GestureType.Rotation)
            {
                HandleRotationGesture(currentAngle);
            }

            // Update tracking values
            lastTouchDistance = currentDistance;
            lastTouchAngle = currentAngle;
        }
        else if (touch1Phase == UnityEngine.InputSystem.TouchPhase.Ended ||
                 touch2Phase == UnityEngine.InputSystem.TouchPhase.Ended ||
                 touch1Phase == UnityEngine.InputSystem.TouchPhase.Canceled ||
                 touch2Phase == UnityEngine.InputSystem.TouchPhase.Canceled)
        {
            currentGesture = GestureType.None;
            totalMovementDistance = 0f;
        }
    }

    private GestureType DetermineGestureFromMovement(Vector2 touch1Movement, Vector2 touch2Movement)
    {
        // Calculate the relative movement between fingers
        // If fingers move towards each other or away from each other, it's zoom
        // If fingers move in the same direction (parallel), it's rotation

        // Get the vector between the two start positions
        Vector2 fingerDirection = (touch2StartPosition - touch1StartPosition).normalized;

        // Project each finger's movement onto the line connecting the fingers
        float touch1ProjectedMovement = Vector2.Dot(touch1Movement, fingerDirection);
        float touch2ProjectedMovement = Vector2.Dot(touch2Movement, fingerDirection);

        // Project each finger's movement perpendicular to the line connecting the fingers
        Vector2 perpendicular = new Vector2(-fingerDirection.y, fingerDirection.x);
        float touch1PerpendicularMovement = Vector2.Dot(touch1Movement, perpendicular);
        float touch2PerpendicularMovement = Vector2.Dot(touch2Movement, perpendicular);

        // Calculate zoom and rotation movement magnitudes
        float zoomMovement = Mathf.Abs(touch1ProjectedMovement - touch2ProjectedMovement);
        float rotationMovement = Mathf.Abs(touch1PerpendicularMovement + touch2PerpendicularMovement);

        // Add minimum threshold to avoid tiny movements
        if (zoomMovement < 5f && rotationMovement < 5f)
        {
            return GestureType.None;
        }

        // Determine gesture based on which type of movement is stronger
        if (zoomMovement > rotationMovement * 1.2f) // 1.2f adds slight bias toward zoom
        {
            return GestureType.Zoom;
        }
        else if (rotationMovement > zoomMovement * 1.2f) // 1.2f adds slight bias toward rotation
        {
            return GestureType.Rotation;
        }

        return GestureType.None; // Ambiguous movement
    }

    private void HandleZoomGesture(float currentDistance)
    {
        if (lastTouchDistance > 0)
        {
            float deltaDistance = currentDistance - lastTouchDistance;
            float zoomDelta = (deltaDistance / Screen.height) * zoomSpeed;

            Vector3 newScale = mapTransform.localScale + Vector3.one * zoomDelta;
            newScale = Vector3.Max(Vector3.one * minZoom, Vector3.Min(Vector3.one * maxZoom, newScale));

            mapTransform.localScale = newScale;
        }
    }

    private void HandleRotationGesture(float currentAngle)
    {
        float angleDelta = Mathf.DeltaAngle(lastTouchAngle, currentAngle);
        mapTransform.Rotate(0, 0, angleDelta * rotationSpeed);
    }

    // Helper methods for Input System
    private UnityEngine.InputSystem.Controls.TouchControl GetFirstActiveTouch()
    {
        if (touchscreen == null) return null;

        var touches = touchscreen.touches;
        for (int i = 0; i < touches.Count; i++)
        {
            if (touches[i].isInProgress)
                return touches[i];
        }
        return null;
    }

    private System.Collections.Generic.List<UnityEngine.InputSystem.Controls.TouchControl> GetActiveTouches()
    {
        var activeTouches = new System.Collections.Generic.List<UnityEngine.InputSystem.Controls.TouchControl>();

        if (touchscreen == null) return activeTouches;

        var touches = touchscreen.touches;
        for (int i = 0; i < touches.Count; i++)
        {
            if (touches[i].isInProgress)
                activeTouches.Add(touches[i]);
        }
        return activeTouches;
    }
}