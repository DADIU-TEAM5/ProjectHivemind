using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : GameLoop
{
    public float MoveLength = 1f;
    public override void LoopLateUpdate(float deltaTime)
    {
    }

    public override void LoopUpdate(float deltaTime)
    {
        transform.position += new Vector3(0, 0, MoveLength * deltaTime);
    }
}
