using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Transform Player;
    public Vector3Variable PlayerDirectionSO;

    [Tooltip("Maximum Speed in m/s")]
    public float MoveSpeed = 1f;
    [Tooltip("Acceleration time in seconds")]
    public float RampupTime = 1f;


    private Vector3 _input;
    private float _lerpTime = 0f;
    private Vector3 _velocity;


    // Start is called before the first frame update
    void Start()
    {
        _input = new Vector3();
        _velocity = new Vector3();
    }


    // Update is called once per frame
    void Update()
    {

        // Assign controller input to _input vector
        _input.x = Input.GetAxisRaw("Horizontal");
        _input.y = 0;
        _input.z = Input.GetAxisRaw("Vertical");

        // SmoothDamp from 0 to 1 on movement
        if(_input.sqrMagnitude != 0)
        {
            Vector3.SmoothDamp(Vector3.zero, _input, ref _velocity, RampupTime);
        } else
        {
            _velocity = Vector3.zero;
        }

        // Move player using translate
        Player.Translate(_velocity * MoveSpeed * Time.deltaTime);

    }
}
