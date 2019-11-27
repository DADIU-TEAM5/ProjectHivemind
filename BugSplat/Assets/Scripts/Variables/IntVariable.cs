using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Int")]
public class IntVariable : ScriptableObject
{
    // The Current value of the object, used in runtime
    public int Value;

    // Min and Max both used in runtime and in editor
    public int Min = 0;
    public int Max = 99;

    // The intial value of the object. This should be adjustable in the editor 
    public int InitialValue;


    public void SetValue(int NewValue)
    {
        Value = Mathf.Max(Mathf.Min(NewValue, Max), Min);
    }

    public void ResetValue()
    {
        Value = InitialValue;
    }

    


    public void Increase(int delta) {
        Value += delta;
    }
}
