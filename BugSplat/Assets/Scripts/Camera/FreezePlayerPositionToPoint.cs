using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePlayerPositionToPoint : MonoBehaviour
{
    public Transform PlayerGameObject;
    public Transform TargetPosition;
    

    // Update is called once per frame
    void Update()
    {
        PlayerGameObject.position = TargetPosition.position;
    }
}
