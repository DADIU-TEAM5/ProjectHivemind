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
    private bool _obstacleDetectionDone = false;
    private bool _cannotDodge;
    private float _moveDistance;
    public GameObject TestCube;
    public GameObject TestCube2;

    // Initial speed of dash
    public float DashLength;

    // Number of frames the dash lasts
    public float DashSpeed;

    private Vector3 _dashDirection;

    private void Start()
    {
        IsDodging.Value = true;
        DashSpeedSO.Value = DashSpeed; // REMOVE THESE ONCE THE SETTINGS WINDOW IS DONE
        DashLengthSO.Value = DashLength; // REMOVE THESE ONCE THE SETTINGS WINDOW IS DONE
        _moveDistance = DashLengthSO.Value;
    }


    public override void LoopUpdate(float deltaTime)
    {

        if (IsDodging.Value == true)
        {
            Vector3 newPosition = _dashDirection;
                        
            if (!_obstacleDetectionDone)
            {
                Debug.Log("DETECTION ON!");

                _obstacleDetectionDone = true;

                RaycastHit[] foundHits = ObstacleDetection(transform.position - (Vector3.up * 0.5f), transform.position + (Vector3.up * 0.5f), .1f, PlayerDirectionSO.Value, DashLengthSO.Value);

                if (foundHits != null)
                {
                    float distanceToObject = float.MaxValue;
                    Vector3 startPos = new Vector3(transform.position.x, 0, transform.position.z);
                    TestCube.transform.position = startPos;

                    for (int i = 0; i < foundHits.Length; i++)
                    {
                        if(foundHits[i].collider != null)
                        {
                            Debug.Log("Object: " + foundHits[i].collider.gameObject.name);
                            float newDist = Vector3.Distance(foundHits[i].point, startPos);

                            Debug.Log("Point: " + foundHits[i].point);

                            if (newDist < distanceToObject)
                            {
                                Debug.Log("newDist: " + newDist);
                                distanceToObject = newDist;
                            }

                            TestCube2.transform.position = new Vector3(foundHits[i].transform.position.x, 0, foundHits[i].transform.position.z);
                            Debug.Log("DISTANCE CUBES: " + Vector3.Distance(TestCube.transform.position, TestCube2.transform.position));
                        }
                    }
                    
                    Debug.Log("distanceToObject: " + distanceToObject);
                    if (distanceToObject > DashLengthSO.Value)
                    {
                        _moveDistance = DashLengthSO.Value;
                    }
                    else
                    {
                        _moveDistance = distanceToObject;
                    }
                }
                else
                {
                    Debug.Log("No obstacles found!");
                    _moveDistance = DashLengthSO.Value;
                }
                player.transform.Translate(PlayerVelocitySO.Value * _moveDistance);
            }

            /*if (_lerpTime - _currentTime < DashSpeedSO.Value)
            {
                _lerpTime = Time.time;
                float _diffTime = _lerpTime - _currentTime;
                Debug.Log("moveDistance " + _moveDistance);
                PlayerVelocitySO.Value = Vector3.Lerp(Vector3.zero, PlayerDirectionSO.Value * _moveDistance, _diffTime / DashSpeedSO.Value);
                
                player.transform.Translate(PlayerVelocitySO.Value * _moveDistance * Time.deltaTime);
                PlayerDirectionSO.Value = newPosition;
            }
            else
            {
                IsDodging.Value = false;
                _obstacleDetectionDone = false;
                _lerpTime = 0f;
            }*/
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
            _obstacleDetectionDone = false;
        }
    }


    private void DashCurve()
    {


    }


    private RaycastHit[] ObstacleDetection(Vector3 p1, Vector3 p2, float radius, Vector3 direction, float maxDistance) 
    {
        Debug.Log("RAYCAST CALLED!");
        RaycastHit[] hits = Physics.CapsuleCastAll(p1, p2, radius, direction, maxDistance);

        RaycastHit[] obstacleHits = new RaycastHit[hits.Length];

        if (hits.Length > 0)
        {
            int count = 0;

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.gameObject.layer != 8 && hits[i].collider.gameObject.layer != 9 && hits[i].collider.gameObject.layer != 10) // if the collided element is not "Enemy", "Player", 
                {
                    obstacleHits[count] = hits[i];
                    count++;
                }
            }

            if(count > 0)
            {
                return obstacleHits;
            }
            else
            {
                return null;
            }
        } 
        else
        {
            return null;
        } 
    }

}
