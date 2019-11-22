using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public Vector3 TargetPos;
    public Transform ObjectToMove;


    public void MoveToTarget()
    {
        ObjectToMove.localPosition = TargetPos;
    }
}
