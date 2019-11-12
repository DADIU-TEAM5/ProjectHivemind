using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AnimClip : ScriptableObject
{
    public string Name;

    public float ClipLengthInMilliseconds;

    public List<AnimationFrame> Frames;
}