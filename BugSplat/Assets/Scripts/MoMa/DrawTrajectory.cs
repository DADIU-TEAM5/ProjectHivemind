using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTrajectory : MonoBehaviour
{
    public CapsuleScriptObject capsuleScriptObject;

    public Transform transformToShowFrom;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        for (int i = 0; i < capsuleScriptObject.Capsule.TrajectoryHistory.Length; i++)
        {
            Gizmos.DrawSphere(transformToShowFrom.TransformVector(capsuleScriptObject.Capsule.TrajectoryHistory[i])+ transformToShowFrom.position, 0.1f);
        }

        Gizmos.color = Color.blue;
        for (int i = 0; i < capsuleScriptObject.Capsule.TrajectoryFuture.Length; i++)
        {
            Gizmos.DrawSphere(transformToShowFrom.TransformVector(capsuleScriptObject.Capsule.TrajectoryFuture[i]) + transformToShowFrom.position, 0.1f);
        }
    }
}
