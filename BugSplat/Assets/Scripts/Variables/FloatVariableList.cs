using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Float List")]
public class FloatVariableList : ScriptableObject
{
    public List<FloatVariable> Value = new List<FloatVariable>();

}
