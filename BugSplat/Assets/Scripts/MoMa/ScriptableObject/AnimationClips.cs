using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//hold animation clips for communication with multiple scripts
[CreateAssetMenu]
public class AnimationClips : ScriptableObject
{
    public List<AnimClip> AnimClips;
}
