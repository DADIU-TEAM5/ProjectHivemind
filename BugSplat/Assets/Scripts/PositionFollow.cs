using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionFollow : GameLoop
{
    public Transform Target;

    private Vector3 _posDiff;

    void Start() {
        _posDiff = transform.position - Target.position;
    }

    public override void LoopUpdate(float deltaTime)
    {
        var designatedPosition = Target.position + _posDiff;

        transform.position = Target.position + _posDiff;
    }

    public override void LoopLateUpdate(float deltaTime)
    {
    }
}
