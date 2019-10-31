using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Float")]
public class FloatVariable : ScriptableObject
{
    // The Current value of the object, used in runtime
    public float Value;

    // Min and Max both used in runtime and in editor
    public float Min = 0;
    public float Max = 99;

    // The intial value of the object. This should be adjustable in the editor 
    public float InitialValue;


    public void SetValue(float NewValue)
    {
        Value = Mathf.Max(Mathf.Min(NewValue, Max), Min);
    }

    public void ResetValue()
    {
        Value = InitialValue;
    }

    public void OnEnable()
    {
        Value = InitialValue;
    }
}
