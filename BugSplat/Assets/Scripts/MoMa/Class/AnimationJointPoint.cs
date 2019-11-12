using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AnimationJointPoint 
{
    public Vector3 Position;
    public Quaternion Rotation;
    public string Name;

    public Quaternion BaseRotation;


    //add more
    public Vector3 LocalPosition;
    public Quaternion LocalRotation;
    public Vector3 Velocity;
    public int BoneIndex;
}
