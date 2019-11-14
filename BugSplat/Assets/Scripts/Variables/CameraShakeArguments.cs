using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/CameraShakeArguments")]
public class CameraShakeArguments : ScriptableObject
{
    public float ShakeAmplitude;
    public float ShakeFrequency;
    public float ShakeDuration;
}
