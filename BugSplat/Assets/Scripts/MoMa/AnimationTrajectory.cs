using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrajectory : PreProcess
{

    public static void ObtainRootFromAnim(float second, int saveInSecond, int frameRate,
                                    AnimClip animClip, int animIndex, int speed, List<Capsule> capsules, float maxSpeedInAnim)
    {

        var saveGap = (int)(second * frameRate / saveInSecond);
        var startFrame = saveGap * saveInSecond;
        var endFrame = animClip.Frames.Count - saveGap * saveInSecond;
        //var capsules = new List<Capsule>();

        for (int index = startFrame; index < endFrame; index++)
        {
            var capsule = new Capsule();
            var positions = new List<Vector3>();
            var fureturepositions = new List<Vector3>();
            var historypositions = new List<Vector3>();

            var currentJoint = animClip.Frames[index].JointPoints.Find(x => x.Name.Contains("Hips"));
            capsule.CurrentPosition = currentJoint.Position;
            var currentRotation = currentJoint.Rotation;


            GetTrajectory(saveInSecond, saveGap, capsule,
                            animClip, index, speed,
                            fureturepositions, historypositions, maxSpeedInAnim);

            //assign values
            capsule.TrajectoryFuture = fureturepositions.ToArray();
            capsule.TrajectoryHistory = historypositions.ToArray();
            capsule.AnimClipName = animClip.Name;
            capsule.FrameNum = index;
            capsule.AnimClipIndex = animIndex;
            //key joints
            //for (int i = 0; i < animClip.Frames[index].JointPoints.Count; i++)
            //{
            //    capsule.KeyJoints.Add(animClip.Frames[index].JointPoints[i]);
            //}
            GetKeyJoints(animClip.Frames[index], ref capsule);
            capsules.Add(capsule);
        }

    }

    private static void GetTrajectory(int saveInSecond, int saveGap, Capsule currentCapsule,
                                AnimClip animClip, int index, int speed,
                                List<Vector3> fureturepositions,
                                List<Vector3> historypositions, float maxSpeedInAnim)
    {
        for (int i = 0; i < saveInSecond; i++)
        {
            var futureindex = index + i * saveGap;
            var furetureJoint = animClip.Frames[futureindex].JointPoints.Find(x => x.Name.Contains("Hips"));


            var futureRelativePos = (furetureJoint.Position - currentCapsule.CurrentPosition) * speed * maxSpeedInAnim;
            //futureRelativePos.y = 0; // assum we have no jump now
            var futureRotatedBackPos = Quaternion.Inverse(furetureJoint.Rotation) * futureRelativePos;
            futureRotatedBackPos.y = 0;
            fureturepositions.Add(futureRotatedBackPos);


            //same for history
            var historyIndex = index - i * saveGap;
            var hisJoint = animClip.Frames[historyIndex].JointPoints.Find(x => x.Name.Contains("Hips"));

            var hisRelativePos = (hisJoint.Position - currentCapsule.CurrentPosition) * speed * maxSpeedInAnim;
            //hisRelativePos.y = 0;
            var hisRotatedBackPos = Quaternion.Inverse(hisJoint.Rotation) * hisRelativePos;
            hisRotatedBackPos.y = 0;
            historypositions.Add(hisRotatedBackPos);
        }
    }

    private static void GetKeyJoints(AnimationFrame animationFrame, ref Capsule capsule)
    {
        capsule.KeyJoints = new List<AnimationJointPoint>();
        for (int i = 0; i < animationFrame.JointPoints.Count; i++)
        {
            if (animationFrame.JointPoints[i].Name == "Hips")
                continue;

            capsule.KeyJoints.Add(animationFrame.JointPoints[i]);
        }
    }

}
