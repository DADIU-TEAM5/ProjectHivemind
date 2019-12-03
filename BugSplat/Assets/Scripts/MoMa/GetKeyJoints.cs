using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetKeyJoints : PreProcess
{

    public static void RootJointsToKeyJoints(AnimClip anim, List<string> keyJointNames)
    {
        for (int i = 0; i < anim.Frames.Count; i++)
        {
            var newJoints = new List<AnimationJointPoint>();
            for (int j = 0; j < anim.Frames[i].JointPoints.Count; j++)
                if (keyJointNames.Contains(anim.Frames[i].JointPoints[j].Name))
                    newJoints.Add(anim.Frames[i].JointPoints[j]);
            anim.Frames[i].JointPoints = newJoints;
        }
    }
}
