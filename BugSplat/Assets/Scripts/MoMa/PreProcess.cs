using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

using UnityEditor.Animations;

public class PreProcess : MonoBehaviour
{
    public AnimationClips AnimationsPlay;
    public AnimationClips AllAnimations;
    public AnimationCapsules AnimationsPreProcess;
    public List<string> MagicMotionNames;
    public MagicMotions MagicMotions;
    public MagicMotion MagicMotion;

    public float Second = 1f;
    public int SaveInSecond = 10;
    public int Speed = 5;
    //Assume we know the frame rate is 30;
    public int FrameRate = 30;


    public void PreProcessTrajectory()
    {
        int count = 0;
        InitializeAnimation();
        for(int i = 0; i < AllAnimations.AnimClips.Count; i++)
        {
            if(AllAnimations.AnimClips[i].Name.Contains("InPlace"))
            {
                AnimationsPlay.AnimClips.Add(AllAnimations.AnimClips[i]);
                GetCorrespondingAnimations(AllAnimations.AnimClips[i].Name, count);
                count++;
            }
        }
        GetMagicMotion();
    }

    private void InitializeAnimation()
    {
        var animclips = new List<AnimClip>();
        AnimationsPlay.AnimClips = animclips;
        AnimationsPreProcess.FrameCapsules = new List<Capsule>();
    }


    private void GetAnimaitionTrajectory(AnimClip animClip, int animIndex)
    {
        AnimationTrajectory.ObtainRootFromAnim(Second, SaveInSecond, FrameRate,
                      animClip, animIndex, Speed, AnimationsPreProcess.FrameCapsules);
    }

    private void GetCorrespondingAnimations(string inPlaceAnimationName, int animIndex)
    {
        var name = inPlaceAnimationName.Replace("_InPlace", "");
        for (int i = 0; i < AllAnimations.AnimClips.Count; i++)
        {
            if (AllAnimations.AnimClips[i].Name == name)
            {
                GetAnimaitionTrajectory(AllAnimations.AnimClips[i], animIndex);
                
                break;
            }
        }
    }

    private void GetMagicMotion()
    {
        MagicMotions.AttackMotions = new List<MagicMotion>();
        for (int j = 0; j < AnimationsPreProcess.FrameCapsules.Count; j++)
            for (int i = 0; i < MagicMotionNames.Count; i++)
            {
                if (AnimationsPreProcess.FrameCapsules[j].AnimClipName.Contains(MagicMotionNames[i])
                    && AnimationsPreProcess.FrameCapsules[j].FrameNum == (int)(Second * FrameRate))
                {
                    //MagicMotion = new MagicMotion();
                    MagicMotion.AnimClipName = AnimationsPreProcess.FrameCapsules[j].AnimClipName;
                    MagicMotion.CapsuleNum = j;
                    MagicMotions.AttackMotions.Add(MagicMotion);
                }
            }
    }

}
