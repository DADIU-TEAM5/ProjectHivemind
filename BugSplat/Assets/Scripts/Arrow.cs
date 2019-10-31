using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : GameLoop
{



    GameObject _arrow;
    LineRenderer _arrowRenderer;

    private void Start()
    {
        _arrow = new GameObject();


        _arrow.AddComponent<LineRenderer>();
        _arrowRenderer = _arrow.GetComponent<LineRenderer>();

        //_arrow.SetActive(false);

        DrawArrow();
    }

    public override void LoopLateUpdate(float deltaTime)
    {
        
    }

    public override void LoopUpdate(float deltaTime)
    {
        
    }

    void DrawArrow()
    {
        Vector3[] pointsForArrow = new Vector3[6];
        _arrowRenderer.positionCount = 6;

        pointsForArrow[0] = transform.TransformPoint(Vector3.back);





        pointsForArrow[1] = transform.TransformPoint(Vector3.forward *1.3f);

        pointsForArrow[2] = transform.TransformPoint(Vector3.forward*.7f + Vector3.right*.5f);

        pointsForArrow[3] = transform.TransformPoint(Vector3.forward * 1.3f);

        pointsForArrow[4] = transform.TransformPoint(Vector3.forward * .7f + Vector3.left * .5f);

        pointsForArrow[5] = transform.TransformPoint(Vector3.forward * 1.3f);





        _arrowRenderer.SetPositions(pointsForArrow);
        _arrowRenderer.widthMultiplier = 0.1f;

        //_coneRenderer.loop = true;
    }

}
