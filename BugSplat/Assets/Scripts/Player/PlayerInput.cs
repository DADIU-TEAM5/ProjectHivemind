using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class PlayerInput : MonoBehaviour
{
    public InputMaster InputActions;

    public float SwipeThreshold;
    public int SwipeHistoryCheck;

    [Header("Dependencies")]
    public BoolVariable PlayerControlOverride;
    public BoolVariable IsStunned;

    public Vector3Variable MoveDirection;


    [Header("Events")]
    public GameEvent AttackEvent;
    public GameEvent DashEvent;

    private Vector2 _pointerStartPos = Vector2.zero;

    private float _swipeThreshold;

    void Awake() {
        InputActions = new InputMaster();
        EnhancedTouchSupport.Enable();

        _swipeThreshold = Mathf.RoundToInt(Screen.width * SwipeThreshold);

        InputActions.PlayerMovement.Movement.performed += context => {
            var delta = context.ReadValue<Vector2>() - _pointerStartPos;
            SetMove(new Vector3(delta.x, 0f, delta.y).normalized);
        };

        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += context => {
            _pointerStartPos = context.screenPosition;
        };

        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove += context => {
            var direction = context.screenPosition - _pointerStartPos;

            SetMove(new Vector3(direction.x, 0f, direction.y).normalized);
        };

        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += context => {
            _pointerStartPos = Vector2.zero;
            SetMove(Vector3.zero);
        };

        InputActions.PlayerMovement.Movement.canceled += _ => SetMove(Vector3.zero);

        InputActions.PlayerMovement.Attack.performed += _ => {
            Debug.Log("Tap");
            if (!(PlayerControlOverride.Value || IsStunned.Value)) {
                if (UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count > 0) {
                    var touch = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches[0];
                    if (Vector2.Distance(touch.startScreenPosition, touch.screenPosition) >= _swipeThreshold) DashEvent.Raise();
                    else AttackEvent.Raise();
                }
                else AttackEvent.Raise();
            }
        };

        InputActions.PlayerMovement.Dash.performed += _ => {
            Debug.Log("Dash");
            if (!(PlayerControlOverride.Value || IsStunned.Value)) {
                DashEvent.Raise();
            }
        };
    }

    private void SetMove(Vector3 direction) {
        MoveDirection.Value = (PlayerControlOverride.Value || IsStunned.Value) ? Vector3.zero : direction;
    }

    void OnEnable() {
        InputActions.Enable();
    }

    void OnDisable() {
        InputActions.Disable();
    }
}
