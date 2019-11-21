using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePlayerPositionToPoint : MonoBehaviour
{
    public BoolVariable TutorialIsActive;
    public Transform PlayerGameObject;
    public Transform TargetPosition;
    public FloatVariable PlayerCurrentSpeedSO;
    public Vector3Variable PlayerDirectionSO;
    public BoolVariable PlayerControlOverrideSO;
     

    // Update is called once per frame
    void OnEnable()
    {
        if(TutorialIsActive.Value == true)
        {
            PlayerControlOverrideSO.Value = true;
            PlayerCurrentSpeedSO.Value = 0;
            //PlayerDirectionSO.Value = Vector3.zero;
            PlayerGameObject.position = TargetPosition.position;
            PlayerGameObject.parent = TargetPosition;
            PlayerGameObject.GetComponentInChildren<PlayerTrajectory>().MoMaUpdateTime = 100;
        }
    }

    public void Reset()
    {
        PlayerCurrentSpeedSO.Value = 0;
        PlayerGameObject.parent = null;
        PlayerGameObject.GetComponentInChildren<PlayerTrajectory>().MoMaUpdateTime = 0.2f;
    }
}
