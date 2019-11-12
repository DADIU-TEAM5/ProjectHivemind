using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MoMaSetting : ScriptableObject
{
    public float Second = 1f; //how long you want to save the trajectory
    public float SaveInSecond = 10; //how many node you want to save in trajectory;

    public string[] KeyJoints = { "Hips", "LeftUpLeg", "RightUpLeg"};


}
