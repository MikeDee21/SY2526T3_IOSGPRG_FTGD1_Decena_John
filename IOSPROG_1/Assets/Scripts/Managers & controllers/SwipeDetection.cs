using UnityEngine;
using UnityEngine.InputSystem;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;

    private InputAction _touchPressedAction;
    private InputAction _touchPositionAction;

    private Vector2 _touchStart;
    private Vector2 _touchEnd;

    [SerializeField] private float _swipeThreshold = 50f;

    private void Awake()
    {
        Debug.Log("SwipeDetection Awake");

        _touchPressedAction = _playerInput.actions["TouchPress"];
        _touchPositionAction = _playerInput.actions["TouchPosition"];

        Debug.Log($"TouchPress Action: {_touchPressedAction}");
        Debug.Log($"TouchPosition Action: {_touchPositionAction}");
    }

    private void OnEnable()
    {
        Debug.Log("SwipeDetection Enabled");

        _touchPressedAction.Enable();
        _touchPositionAction.Enable();

        _touchPressedAction.started += OnTouchStarted;
        _touchPressedAction.canceled += OnTouchReleased;

        Debug.Log("Input Actions Enabled + Subscribed");
    }

    private void OnDisable()
    {
        Debug.Log("SwipeDetection Disabled");

        _touchPressedAction.started -= OnTouchStarted;
        _touchPressedAction.canceled -= OnTouchReleased;

        _touchPressedAction.Disable();
        _touchPositionAction.Disable();
    }

    private void OnTouchStarted(InputAction.CallbackContext context)
    {
        Debug.Log("Touch Started");

        _touchStart = _touchPositionAction.ReadValue<Vector2>();

        Debug.Log($"Touch Start Position: {_touchStart}");
    }

    private void OnTouchReleased(InputAction.CallbackContext context)
    {
        Debug.Log("Touch Released");

        _touchEnd = _touchPositionAction.ReadValue<Vector2>();

        Debug.Log($"Touch End Position: {_touchEnd}");

        Vector2 swipeDelta = _touchEnd - _touchStart;

        Debug.Log($"Swipe Delta: {swipeDelta}");
        Debug.Log($"Swipe Magnitude: {swipeDelta.magnitude}");

        if (swipeDelta.magnitude < _swipeThreshold)
        {
            Debug.Log("Swipe too small, ignored");
            return;
        }

        SwipeDirection swipeDirection;

        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            swipeDirection = swipeDelta.x > 0
                ? SwipeDirection.Right
                : SwipeDirection.Left;
        }
        else
        {
            swipeDirection = swipeDelta.y > 0
                ? SwipeDirection.Up
                : SwipeDirection.Down;
        }

        Debug.Log($"Detected Swipe Direction: {swipeDirection}");

        if (Spawner.Instance.CurrentEnemy != null)
        {
            Debug.Log("CurrentEnemy found, sending swipe");

            Spawner.Instance.CurrentEnemy.CheckSwipe(swipeDirection);
        }
        else
        {
            Debug.LogWarning("No CurrentEnemy detected");
        }
    }
}