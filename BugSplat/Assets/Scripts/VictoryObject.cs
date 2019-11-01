using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryObject : MonoBehaviour
{
    
    public GameObjectVariable Victory;

    private void OnEnable()
    {
        Victory.Value = gameObject;
    }
}
