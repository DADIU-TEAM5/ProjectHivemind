//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Linq;

//public class BlendAnimations : PlayerTrajectory
//{

//    public void PlayBlendAnimation(Dictionary<string, Transform> skeletonJoints, float blendDegree,
//                            int beginFrameIndex, int beginAnimIndex, Result result, CapsuleScriptObject PlayerTrajectoryCapusule,
//                            AnimationClips animationClips, int areadlyBlendedTimes, Vector3 rotationEular)

//    {
//        int bestFrameIndex = result.FrameNum;
//        PlayerTrajectoryCapusule.Capsule.AnimClipIndex = result.AnimClipIndex;
//        PlayerTrajectoryCapusule.Capsule.AnimClipName = result.ClipName;
//        PlayerTrajectoryCapusule.Capsule.FrameNum = result.FrameNum;

//        if (result.FrameNum >= animationClips.AnimClips[result.AnimClipIndex].Frames.Count - 3)//3 should be start frame 
//            result.FrameNum = 3; //3 should be start frame 

//        BlendAnimation(beginFrameIndex, bestFrameIndex, skeletonJoints,
//            areadlyBlendedTimes, animationClips.AnimClips[beginAnimIndex], animationClips.AnimClips[result.AnimClipIndex], blendDegree);
//        transform.Rotate(rotationEular);
//    }


//    private void BlendAnimation(int beginFrameIndex, int bestFrameIndex, Dictionary<string, Transform> skeletonJoints,
//                            int areadlyBlendedTimes, AnimClip beginClip, AnimClip bestClip, float blendDegree)
//    {
//        var blendStart = beginFrameIndex + areadlyBlendedTimes;
//        var blendEnd = bestFrameIndex + areadlyBlendedTimes;

//        BlendFrame(skeletonJoints, transform, beginClip.Frames[blendStart], bestClip.Frames[blendEnd], blendDegree);
//    }

//    private void BlendFrame(Dictionary<string, Transform> skeletonJoints, Transform transform,
//                                    AnimationFrame startFrame, AnimationFrame endFrame, float blendDegree)
//    {
//        for (int i = 0; i < startFrame.JointPoints.Count; i++)
//        {
//            var startJoint = startFrame.JointPoints[i];
//            var endJoint = endFrame.JointPoints[i];

//            var joint = skeletonJoints[startJoint.Name];
//            BlendJoints(startJoint, endJoint, joint, blendDegree, blendDegree);
//        }
//    }



//    private void BlendJoints(AnimationJointPoint startjointPoint, AnimationJointPoint endjointPoint,
//                             Transform joint, float blendRate, float blendDegree)
//    {

//        joint.rotation = transform.rotation * Quaternion.Lerp(startjointPoint.Rotation, endjointPoint.Rotation, blendDegree);
//        //more cost?
//        joint.position = Vector3.Lerp(transform.TransformDirection(startjointPoint.Position) + transform.position,
//                                        transform.TransformDirection(endjointPoint.Position) + transform.position, blendRate);
//    }
//}
