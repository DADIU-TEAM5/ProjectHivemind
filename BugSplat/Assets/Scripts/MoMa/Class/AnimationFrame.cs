using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimationFrame  
{
    //for debug
    //public string ClipName;

    public float velocity;
    public string[] Keybones;
    public TempMotionTrajectory[] tempMotionTrajectory;


    public float Time;
    public List<AnimationJointPoint> JointPoints;
}
