using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MotionMatcher
{

    public void GetMotionAndFrame(string attackTag, MagicMotions magicMotions, AnimationCapsules animationCapsules, CapsuleScriptObject current,
                                    Result result, AnimationClips animationClips, int differentClipLength)
    {
        int bestCapsuleIndex = 0;
        if (attackTag == null)
        {
            bestCapsuleIndex = CalculateCost.GetBestFrameIndex(animationCapsules, current.Capsule, animationClips, magicMotions);
        }
        else
        {
            //
            bestCapsuleIndex = FindTagCapsule(attackTag, magicMotions);
            if (bestCapsuleIndex == 7878)
                bestCapsuleIndex = 30;
        }

        var bestFrame = animationCapsules.FrameCapsules[bestCapsuleIndex];
        bool isSameLocation = (bestCapsuleIndex == result.CapsuleNum)
                                || ((bestFrame.AnimClipIndex == result.AnimClipIndex)
                                && (Mathf.Abs(bestFrame.FrameNum - result.FrameNum) < differentClipLength));


        if (!isSameLocation)
        {
            result.ClipName = bestFrame.AnimClipName;
            result.FrameNum = bestFrame.FrameNum;
            result.CapsuleNum = bestCapsuleIndex;
            result.AnimClipIndex = bestFrame.AnimClipIndex;
        }
        else
        {
            result.FrameNum++;
        }
    }

    //we cannot find the animation every time
    private int FindTagCapsule(string attackTag, MagicMotions magicMotions)
    {
        for (int i = 0; i < magicMotions.AttackMotions.Count; i++)
        {
            if (magicMotions.AttackMotions[i].AnimClipName.Contains(attackTag))
            {
                return magicMotions.AttackMotions[i].CapsuleNum;
            }
        }

        return 7878;
    }



}
