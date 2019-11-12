using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Result : ScriptableObject
{
    public string ClipName;
    public int FrameNum;
    public int AnimClipIndex;

    //for debug
    public int CapsuleNum;
}
