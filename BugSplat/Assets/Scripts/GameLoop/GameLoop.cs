using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameLoop : MonoBehaviour  
{
    [HideInInspector]
    public bool IsPaticipant;

    public abstract void LoopUpdate(float deltaTime);    
    public abstract void LoopLateUpdate(float deltaTime);
    
}
