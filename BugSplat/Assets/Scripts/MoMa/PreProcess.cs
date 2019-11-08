using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

//using UnityEditor.Animations;
using System;

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
    private float _maxSpeedInAnim = 0.001f;

    public void PreProcessTrajectory()
    {
        int count = 0;
        InitializeAnimation();
        for (int i = 0; i < AllAnimations.AnimClips.Count; i++)
        {
            if (AllAnimations.AnimClips[i].Name.Contains("InPlace"))
            {
                AnimationsPlay.AnimClips.Add(AllAnimations.AnimClips[i]);
                GetCorrespondingAnimations(AllAnimations.AnimClips[i].Name, count, false);
                count++;
            }
            // if(!IsMagicMotion(AllAnimations.AnimClips[i].Name))
            // {
            //     AnimationsPlay.AnimClips.Add(AllAnimations.AnimClips[i]);
            //     GetCorrespondingAnimations(AllAnimations.AnimClips[i].Name, count, false);
            //     count++;
            // }
            // else
            // {
            //     AnimationsPlay.MagicClips.Add(AllAnimations.AnimClips[i]);
            //     GetCorrespondingAnimations(AllAnimations.AnimClips[i].Name, count,true);
            //     count++;
            // }
        }
        GetMagicMotion();
    }

    // public bool IsMagicMotion(string animName)
    // {
    //     for(int i = 0; i < MagicMotionNames.Count; i++)
    //     {
    //         if(animName.Contains(MagicMotionNames[i]))
    //             return true;
    //     }
    //     return false;
    // }
    private void InitializeAnimation()
    {
        var animclips = new List<AnimClip>();
        AnimationsPlay.AnimClips = animclips;
        AnimationsPreProcess.FrameCapsules = new List<Capsule>();
        // AnimationsPreProcess.MagicCapsules = new List<Capsule>();
        _maxSpeedInAnim = 1f / GetMaxSpeed();

    }


    private void GetAnimaitionTrajectory(AnimClip animClip, int animIndex, bool isMagic)
    {
        // if(!isMagic)
        AnimationTrajectory.ObtainRootFromAnim(Second, SaveInSecond, FrameRate,
                  animClip, animIndex, Speed, AnimationsPreProcess.FrameCapsules, _maxSpeedInAnim);
        // else
        //     AnimationTrajectory.ObtainRootFromAnim(Second, SaveInSecond, FrameRate,
        //               animClip, animIndex, Speed, AnimationsPreProcess.MagicCapsules, _maxSpeedInAnim);
    }

    private void GetCorrespondingAnimations(string inPlaceAnimationName, int animIndex, bool isMagic)
    {
        var name = inPlaceAnimationName.Replace("_InPlace", "");
        for (int i = 0; i < AllAnimations.AnimClips.Count; i++)
        {
            if (AllAnimations.AnimClips[i].Name == name)
            {
                GetAnimaitionTrajectory(AllAnimations.AnimClips[i], animIndex, isMagic);
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

    private float GetMaxSpeed()
    {
        float maxSpeed = 0.01f;

        for (int i = 0; i < AllAnimations.AnimClips.Count; i++)
            if (!AllAnimations.AnimClips[i].Name.Contains("InPlace"))
                for (int j = 1; j < AllAnimations.AnimClips[i].Frames.Count; j++)
                {
                    var joint = AllAnimations.AnimClips[i].Frames[j].JointPoints.Find(x => x.Name.Contains("Root"));
                    var jointBefore = AllAnimations.AnimClips[i].Frames[j - 1].JointPoints.Find(x => x.Name.Contains("Root"));
                    joint.Position.y = 0;
                    jointBefore.Position.y = 0;
                    //distance / time = speed
                    var speed = ((joint.Position - jointBefore.Position) / (1f / FrameRate)).magnitude;
                    if (speed > maxSpeed)
                        maxSpeed = speed;
                }

        //return maxSpeed;
        return 1;
    }
}
