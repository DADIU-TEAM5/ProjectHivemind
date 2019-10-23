using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Transform Player;
    public Vector3Variable PlayerSpeedDirectionSO;
    public FloatVariable PlayerMaxSpeedSO;
    public Vector3Variable PlayerVelocitySO;
    public FloatVariable PlayerAccelerationSO;

    private float _lerpTime = 0f;
    private Vector3 _velocity;


    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

        // SmoothDamp from 0 to 1 on Normal movement
        if(PlayerSpeedDirectionSO.Value.sqrMagnitude != 0)
        {
            Vector3.SmoothDamp(Vector3.zero, PlayerSpeedDirectionSO.Value , ref PlayerVelocitySO.Value, PlayerAccelerationSO.Value);
        } else
        {
            PlayerVelocitySO.Value = Vector3.zero;
        }

        // Move player using translate
        Player.Translate(PlayerVelocitySO.Value * PlayerMaxSpeedSO.Value * Time.deltaTime);

        // Rotate player based on PlayerSpeedDirection
        Player.rotation = Quaternion.LookRotation(PlayerSpeedDirectionSO.Value, Vector3.up);

    }
}
