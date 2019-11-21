using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBool : MonoBehaviour
{

    public BoolVariable TargetBool;


    public void Set(bool chosenValue)
    {
        TargetBool.Value = chosenValue;
    }
}
