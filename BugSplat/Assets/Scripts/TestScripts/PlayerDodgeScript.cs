using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeScript : GameLoop
{
    public Rigidbody player;
    public GameEvent DashInitiated;
    public BoolVariable IsDodging;
    public Vector3Variable PlayerDirectionSO;
    public AnimationCurve DodgeAnimationCurve;
   
    // Initial speed of dash
    public float DashSpeed;

    // Number of frames the dash lasts
    public float DashLength;

    private Vector3 _dashDirection;
    private int _dashFrameCount;

    // Called from inputmanager
    public void StartDash()
    {
        DashInitiated.Raise();
    }

    // Called from PlayerController
    public void PlayerDash()
    {
        
        if (IsDodging.Value == false)
        {
            _dashDirection = PlayerDirectionSO.Value;
            _dashFrameCount = 0;
            IsDodging.Value = true;
        }
    }

    private void dashCurve()
    {

    }

    
    public override void LoopUpdate(float deltaTime)
    {
        Debug.Log("Dodging : " + IsDodging.Value);

        
        if (IsDodging.Value == true)
        {
            Vector3 newPosition = _dashDirection;
            PlayerDirectionSO.Value = newPosition;
            
            // Only for Testing !TEST
            player.MovePosition(player.transform.position + (PlayerDirectionSO.Value * DashSpeed));
            

            
            _dashFrameCount++;

            if (_dashFrameCount >= DashLength)

                // Only for Testing !TEST
                PlayerDirectionSO.Value = _dashDirection;

                IsDodging.Value = false;
        }
       
    }

    public override void LoopLateUpdate(float deltaTime)
    {
       
    }

    


}
