using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Assertions;

using UnityEngine.UI;

[ExecuteInEditMode]
public class ClipsFetch : MonoBehaviour
{
    
    public AnimClip AnimationClip;
    //public AnimClipCSV OriginalClip;
    public AnimClip NewClip;
    public Transform Skeleton;
    public int StartFrame, EndFrame;
    public float value;
    public int index;
    public string ClipName;
    //public string ClipPath = "Assets/TestNew.asset";
    private Dictionary<string, Transform> SkeletonJoints = new Dictionary<string, Transform>();

    void Awake()
    {
        //transf(Skeleton);
        GetAllChildren(Skeleton);
    }

    public void CutAnimation()
    {
        //AnimClip NewClip = ScriptableObject.CreateInstance<AnimClip>();
        //UnityEditor.AssetDatabase.CreateAsset(NewClip, ClipPath);
        //Object o = UnityEditor.AssetDatabase.LoadAssetAtPath(ClipPath, typeof(AnimClip));
        NewClip.Name = ClipName;
        NewClip.Frames.RemoveRange(0, NewClip.Frames.Count);
        for (int i = StartFrame; i < EndFrame; i++)
        {
            if (i < 0)
                break;
            if (i >= AnimationClip.Frames.Count)
                break;

            NewClip.Frames.Add(AnimationClip.Frames[i]);
        }
        
    }
    

    public void GetFrame()

    {
        index = (int)(value * AnimationClip.Frames.Count);
        FrameToJoints(AnimationClip.Frames[index]);
    }

    public void FrameToJoints(AnimationFrame frame)
    {
        //Debug.Log(frame.Velocity);
        //Debug.Log((int)(value * AnimationClip.Frames.Count));
        foreach (var jointPoint in frame.JointPoints)
        {
            if (!SkeletonJoints.Keys.Contains(jointPoint.Name))
            {
                //Debug.LogError($"{jointPoint.Name} is not in the {Skeleton.name}");
                continue;
            }

            var joint = SkeletonJoints[jointPoint.Name];
            ApplyJointPointToJoint(jointPoint, joint);
        }
    }


    private void ApplyJointPointToJoint(AnimationJointPoint jointPoint, Transform joint)
    {
        // Based on negative joint
        var newEulerRot = jointPoint.Rotation * Quaternion.Inverse(jointPoint.BaseRotation);
        //var newEulerRot = jointPoint.Rotation * jointPoint.BaseRotation;
        //joint.rotation = newEulerRot;
        joint.rotation = Skeleton.rotation * jointPoint.Rotation;
        //joint.rotation = Skeleton.rotation * (newEulerRot);
        joint.position = Skeleton.position + jointPoint.Position;

        //joint.SetPositionAndRotation(jointPoint.Position, jointPoint.Rotation);
    }

    private void transf(Transform trans)
    {
        foreach (Transform child in trans)
        {
            if (child.childCount > 0) transf(child);
            if (child.name.Contains("mixamorig:"))
            {
                child.name = child.name.Replace("mixamorig:", "");
            }
        }
    }

    private void GetAllChildren(Transform trans)
    {
        foreach (Transform child in trans)
        {
            if (child.childCount > 0) GetAllChildren(child);
            SkeletonJoints.Add(child.name, child);
        }
    }


}
