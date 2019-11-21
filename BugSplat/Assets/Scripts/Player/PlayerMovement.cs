using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : GameLoop
{
    public Animator Anim;

    public Transform PlayerGraphics;

    public Vector3Variable MoveDirectionSO;
    public Vector3Variable PlayerDirectionSO;
    public FloatVariable PlayerCurrentSpeedSO;
    public FloatVariable PlayerMaxSpeedSO;
    public Vector3Variable PlayerPosition;
    public Vector3Variable PlayerVelocity;

    public BoolVariable Attacking;
    public BoolVariable Dodgeing;

    public AnimationCurve RampUpMovespeed;


    NavMeshAgent _navMeshAgent;

    [SerializeField]
    private float LerpTime = 1f;
    private bool _isMoving;
    private float _currentTime;
    public BoolVariable PlayerControlOverrideSO;

    Rigidbody _rigidbody;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();

        PlayerCurrentSpeedSO.Value = 0;
        _currentTime = 0f;
    }

    public override void LoopUpdate(float deltaTime)
    {
        if (PlayerControlOverrideSO.Value == false)
        {
            var moving = MoveDirectionSO.Value != Vector3.zero;

            //Anim.SetBool("Running", moving);

            if (moving)
            {
                var lerpTime = Mathf.Min(_currentTime / LerpTime, 1f);
                lerpTime = RampUpMovespeed.Evaluate(lerpTime);

                PlayerCurrentSpeedSO.Value = lerpTime * PlayerMaxSpeedSO.Value;
                PlayerDirectionSO.Value = MoveDirectionSO.Value;

                _currentTime += Time.deltaTime;
            }
            else
            {
                _currentTime = 0f;
                PlayerCurrentSpeedSO.Value = 0f;
            }
        }

            PlayerVelocity.Value = PlayerDirectionSO.Value * PlayerCurrentSpeedSO.Value;
            PlayerGraphics.localRotation = Quaternion.LookRotation(PlayerDirectionSO.Value, Vector3.up);

            if (_navMeshAgent.isOnNavMesh)
            {
                if (!_navMeshAgent.hasPath)
                {
                    if (!Attacking.Value && !Dodgeing.Value)

                    {
                        _navMeshAgent.Move(PlayerVelocity.Value * Time.deltaTime);
                    }
                }
            }
    }


    private void FixedUpdate()
    {
       _rigidbody.velocity = Vector3.zero;
        
    }
    public override void LoopLateUpdate(float deltaTime)
    {
        PlayerPosition.Value = transform.position;
    }
}
