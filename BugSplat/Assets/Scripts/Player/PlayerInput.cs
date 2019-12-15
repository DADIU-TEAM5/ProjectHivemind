using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class PlayerInput : MonoBehaviour
{
    public InputMaster InputActions;

    [Header("Dependencies")]
    public BoolVariable PlayerControlOverride;
    public BoolVariable IsStunned;

    public Vector3Variable MoveDirection;


    [Header("Events")]
    public GameEvent AttackEvent;
    public GameEvent DashEvent;

    private Vector2 _pointerStartPos;

    private bool _pointerHold = false;

    void Awake() {
        InputActions = new InputMaster();
        EnhancedTouchSupport.Enable();

        InputActions.PlayerMovement.Movement.performed += context => {
            var delta = (!_pointerHold || PlayerControlOverride.Value || IsStunned.Value) ? Vector2.zero : context.ReadValue<Vector2>();
            MoveDirection.Value = new Vector3(delta.x, 0f, delta.y);
        };

        InputActions.PlayerMovement.Attack.performed += _ => {
            Debug.Log("Tap");
            if (!(PlayerControlOverride.Value || IsStunned.Value)) AttackEvent.Raise();
        };

        InputActions.PlayerMovement.Dash.performed += _ => {
            Debug.Log("Dash");
            _pointerHold = false;
            if (!(PlayerControlOverride.Value || IsStunned.Value)) {
                DashEvent.Raise();
            }
        };

        InputActions.PlayerMovement.PointerStart.performed += context => {
            Debug.Log("PointerStart");
            _pointerStartPos = context.ReadValue<Vector2>();
            _pointerHold = true;
        };
    }

    void OnEnable() {
        InputActions.Enable();
    }

    void OnDisable() {
        InputActions.Disable();
    }
}
