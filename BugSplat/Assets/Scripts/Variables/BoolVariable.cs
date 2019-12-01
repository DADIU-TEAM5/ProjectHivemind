using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Bool")]
public class BoolVariable : ScriptableObject
{
    public bool Value;

    public void SetFalse()
    {
        Value = false;
    }
    public void SetTrue()
    {
        Value = true;
    }
}
