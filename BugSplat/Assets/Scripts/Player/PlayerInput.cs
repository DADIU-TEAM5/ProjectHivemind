using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class PlayerInput : MonoBehaviour
{
    public InputMaster InputActions;

    public float SwipeThreshold;

    public float PointerMinMove, PointerMaxMove;

    [Header("Dependencies")]
    public BoolVariable PlayerControlOverride;
    public BoolVariable IsStunned;

    public Vector3Variable MoveDirection;

    public GameObject InputUI;
    public GameObject InputUIUpper;


    [Header("Events")]
    public GameEvent AttackEvent;
    public GameEvent DashEvent;

    private Vector2 _pointerStartPos = Vector2.zero;

    private float _swipeThreshold;
    private float _pointerMinMove, _pointerMaxMove;

    void Awake() {
        InputActions = new InputMaster();
        EnhancedTouchSupport.Enable();

        _swipeThreshold = Mathf.RoundToInt(Screen.width * SwipeThreshold);
        _pointerMinMove = Screen.width * PointerMinMove;
        _pointerMaxMove = Screen.width * PointerMaxMove;

        InputActions.PlayerMovement.Movement.performed += context => {
            var delta = context.ReadValue<Vector2>() - _pointerStartPos;
            SetMove(new Vector3(delta.x, 0f, delta.y).normalized);
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

    private void FingerDown(Finger finger) {
        _pointerStartPos = finger.screenPosition;
        InputUI.SetActive(true);
        InputUI.transform.position = _pointerStartPos;
    }

    private void FingerMove(Finger finger) {
        var heading = finger.screenPosition - _pointerStartPos;
        var distance = heading.magnitude;

        if (distance > _pointerMinMove) {
            // Calculates the outer rim position of the "joystick" if the player moves the finger beyond the borders of the designated joystick space
            var a = Mathf.Atan2(heading.y, heading.x);
            if (distance > _pointerMaxMove)
            {
                heading.x = _pointerMaxMove * Mathf.Cos(a);
                heading.y = _pointerMaxMove * Mathf.Sin(a);
            }

            // UI stuff
            InputUIUpper.transform.localScale = new Vector3(1, Mathf.Clamp(distance*0.03f,1,4), 1);
            InputUI.transform.up = heading;
        }

        SetMove(new Vector3(heading.x, 0f, heading.y).normalized);
    }

    private void FingerUp(Finger finger) {
        _pointerStartPos = Vector2.zero;
        SetMove(Vector3.zero);
        InputUI?.SetActive(false);
    }

    void OnEnable() {
        InputActions.Enable();

        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += FingerDown;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove += FingerMove;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += FingerUp;
    }

    void OnDisable() {
        InputActions.Disable();

        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= FingerDown;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove -= FingerMove;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp -= FingerUp;
    }
}
