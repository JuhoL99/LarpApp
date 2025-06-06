using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class MapTouchController : MonoBehaviour
{
    [Header("Map Settings")]
    public List<Image> mapImages; // List of all floor images
    public RectTransform mapTransform; // The UI Image RectTransform (container for all floors)
    public float minZoom = 0.5f;
    public float maxZoom = 3f;
    public float zoomSpeed = 1f;
    public float rotationSpeed = 1f;

    [Header("Pan Settings")]
    public bool enablePanning = true;
    public float panSpeed = 2f;
    public bool enableBoundaries = true;
    public float boundaryPadding = 0f; // Extra padding from screen edges

    [Header("Floor Management")]
    public int currentFloorIndex = 0; // Which floor is currently active

    [Header("Gesture Detection")]
    public float movementThreshold = 20f; // Minimum movement to start detecting gestures
    public float gestureDeadZone = 15f; // Dead zone angle (degrees) where no gesture is detected

    private Vector2 lastTouchPosition;
    private float lastTouchDistance;
    private float lastTouchAngle;
    private bool isPanning = false;

    // Gesture detection variables
    private enum GestureType { None, Zoom, Rotation }
    private GestureType currentGesture = GestureType.None;
    private Vector2 touch1StartPosition;
    private Vector2 touch2StartPosition;
    private float totalMovementDistance = 0f;

    // Input System references
    private Touchscreen touchscreen;

    // Boundary calculation cache
    private Canvas parentCanvas;
    private RectTransform canvasRectTransform;

    void Start()
    {
        if (mapTransform == null)
            mapTransform = GetComponent<RectTransform>();

        // Get input devices
        touchscreen = Touchscreen.current;

        // Get canvas references for boundary calculations
        parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas != null)
        {
            canvasRectTransform = parentCanvas.GetComponent<RectTransform>();
        }

        // Initialize floor display
        if (mapImages != null && mapImages.Count > 0)
        {
            ShowFloor(currentFloorIndex);
        }
    }

    void Update()
    {
        HandleInput();
    }

    // Public method to switch floors - call this from button OnClick events (first floor = index 0)
    public void SwitchToFloor(int floorIndex)
    {
        currentFloorIndex = floorIndex;
        ShowFloor(currentFloorIndex);
    }

    private void ShowFloor(int floorIndex)
    {
        // Hide all floor images, set selected active
        for (int i = 0; i < mapImages.Count; i++)
        {
            if (mapImages[i] != null)
            {
                mapImages[i].gameObject.SetActive(i == floorIndex);
            }
        }
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
                Vector2 newPosition = mapTransform.anchoredPosition + deltaPosition;

                // Apply boundary constraints
                if (enableBoundaries)
                {
                    newPosition = ConstrainToBoundaries(newPosition);
                }

                mapTransform.anchoredPosition = newPosition;
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

            // After zooming, check if we need to adjust position to stay within boundaries
            if (enableBoundaries)
            {
                mapTransform.anchoredPosition = ConstrainToBoundaries(mapTransform.anchoredPosition);
            }
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

    private List<UnityEngine.InputSystem.Controls.TouchControl> GetActiveTouches()
    {
        var activeTouches = new List<UnityEngine.InputSystem.Controls.TouchControl>();

        if (touchscreen == null) return activeTouches;

        var touches = touchscreen.touches;
        for (int i = 0; i < touches.Count; i++)
        {
            if (touches[i].isInProgress)
                activeTouches.Add(touches[i]);
        }
        return activeTouches;
    }

    // Boundary constraint methods
    private Vector2 ConstrainToBoundaries(Vector2 proposedPosition)
    {
        if (canvasRectTransform == null || mapTransform == null)
            return proposedPosition;

        // Get the current scaled size of the map
        Vector2 mapSize = mapTransform.rect.size * mapTransform.localScale.x;
        Vector2 canvasSize = canvasRectTransform.rect.size;

        // Calculate the boundaries
        Vector2 minBounds = CalculateMinBounds(mapSize, canvasSize);
        Vector2 maxBounds = CalculateMaxBounds(mapSize, canvasSize);

        // Constrain the position
        Vector2 constrainedPosition = new Vector2(
            Mathf.Clamp(proposedPosition.x, minBounds.x, maxBounds.x),
            Mathf.Clamp(proposedPosition.y, minBounds.y, maxBounds.y)
        );

        return constrainedPosition;
    }

    private Vector2 CalculateMinBounds(Vector2 mapSize, Vector2 canvasSize)
    {
        // Calculate minimum bounds (how far left/down the map can go)
        float minX = -(mapSize.x - canvasSize.x) * 0.5f - boundaryPadding;
        float minY = -(mapSize.y - canvasSize.y) * 0.5f - boundaryPadding;

        // If map is smaller than canvas, center it
        if (mapSize.x <= canvasSize.x) minX = 0;
        if (mapSize.y <= canvasSize.y) minY = 0;

        return new Vector2(minX, minY);
    }

    private Vector2 CalculateMaxBounds(Vector2 mapSize, Vector2 canvasSize)
    {
        // Calculate maximum bounds (how far right/up the map can go)
        float maxX = (mapSize.x - canvasSize.x) * 0.5f + boundaryPadding;
        float maxY = (mapSize.y - canvasSize.y) * 0.5f + boundaryPadding;

        // If map is smaller than canvas, center it
        if (mapSize.x <= canvasSize.x) maxX = 0;
        if (mapSize.y <= canvasSize.y) maxY = 0;

        return new Vector2(maxX, maxY);
    }

    // Public method to manually apply boundary constraints (useful after direct position changes)
    public void ApplyBoundaryConstraints()
    {
        if (enableBoundaries)
        {
            mapTransform.anchoredPosition = ConstrainToBoundaries(mapTransform.anchoredPosition);
        }
    }
}