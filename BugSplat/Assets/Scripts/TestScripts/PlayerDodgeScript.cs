using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeScript : GameLoop
{
    public Rigidbody player;
    public GameEvent DashInitiated;
    public BoolVariable IsDodging;
    public Vector3Variable PlayerDirectionSO;
    public Vector3Variable PlayerVelocitySO;
    public FloatVariable DashSpeedSO;
    public FloatVariable DashLengthSO;
    public AnimationCurve DodgeAnimationCurve;

    private float _currentTime;
    private float _lerpTime = 0f;

    // Initial speed of dash
    public float DashLength;

    // Number of frames the dash lasts
    public float DashSpeed;

    private Vector3 _dashDirection;

    private void Start()
    {
        IsDodging.Value = false;
        DashSpeedSO.Value = DashSpeed; // REMOVE THESE ONCE THE SETTINGS WINDOW IS DONE
        DashLengthSO.Value = DashLength; // REMOVE THESE ONCE THE SETTINGS WINDOW IS DONE
    }


    public override void LoopUpdate(float deltaTime)
    {
        //Debug.Log("Dodging : " + IsDodging.Value);


        if (IsDodging.Value == true)
        {
            Vector3 newPosition = _dashDirection;
            float moveDistance = DashLengthSO.Value;

            RaycastHit[] hits = Physics.CapsuleCastAll(transform.position - (Vector3.up * 0.5f), transform.position + (Vector3.up * 0.5f), .1f, PlayerDirectionSO.Value, DashSpeed);
            //RaycastHit hit;
            //if (Physics.CapsuleCast(transform.position - (Vector3.up * 0.5f), transform.position + (Vector3.up * 0.5f), .1f, PlayerDirectionSO.Value, out hit))
            if (hits.Length > 0)
            {
                float distanceToObject = float.MaxValue;
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].collider.gameObject.layer != 8)
                    {
                        float newDist = Vector3.Distance(hits[i].point, transform.position);
                        if (newDist < distanceToObject)
                        {
                            distanceToObject = newDist;
                        }
                    }
                }


                //print(hit.collider.gameObject.name);
                if (distanceToObject > DashLengthSO.Value)
                {
                    moveDistance = DashLengthSO.Value;

                    //transform.Translate(PlayerDirectionSO.Value * DashSpeed);
                }
                else
                {
                    moveDistance = distanceToObject;
                    //transform.Translate(PlayerDirectionSO.Value * distanceToObject);
                }
            }
            /*else
            {

                transform.Translate(PlayerDirectionSO.Value * DashSpeed);
            }*/

            if (_lerpTime - _currentTime < moveDistance)
            {
                _lerpTime = Time.time;

                float _diffTime = _lerpTime - _currentTime;
                Debug.Log(_diffTime);
                PlayerVelocitySO.Value = Vector3.Lerp(PlayerDirectionSO.Value, newPosition * moveDistance, _diffTime / DashSpeed);
            }
            else
            {
                IsDodging.Value = false;
                _lerpTime = 0f;
            }

            player.transform.Translate(PlayerVelocitySO.Value * moveDistance * Time.deltaTime);

            PlayerDirectionSO.Value = newPosition;
        }
    }


    public override void LoopLateUpdate(float deltaTime)
    {

    }

    

    // Called from PlayerController
    public void PlayerDash()
    {
        if (IsDodging.Value == false)
        {
            _dashDirection = PlayerDirectionSO.Value;
            _currentTime = Time.time;
            _lerpTime = 0f;
            IsDodging.Value = true;
        }
    }


    private void DashCurve()
    {


    }




}
