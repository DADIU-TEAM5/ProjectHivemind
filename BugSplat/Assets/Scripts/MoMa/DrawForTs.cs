using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawForTs : MonoBehaviour
{
    
    public AnimationCapsules animationCapsules;

    public Transform transformToShowFrom;
    
    public Result result;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        if (animationCapsules != null)
        {
            for (int i = 0; i < animationCapsules.FrameCapsules[result.CapsuleNum].TrajectoryHistory.Length; i++)
            {
                Gizmos.DrawSphere(transformToShowFrom.TransformVector(animationCapsules.FrameCapsules[result.CapsuleNum].TrajectoryHistory[i]) + transformToShowFrom.position, 0.1f);
            }

            Gizmos.color = Color.green;
            for (int i = 0; i < animationCapsules.FrameCapsules[result.CapsuleNum].TrajectoryFuture.Length; i++)
            {
                Gizmos.DrawSphere(transformToShowFrom.TransformVector(animationCapsules.FrameCapsules[result.CapsuleNum].TrajectoryFuture[i]) + transformToShowFrom.position, 0.1f);
            }
        }
    }
}
