using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutroStart : MonoBehaviour
{
    public IntVariable CurrentLevelSO;
    public GameEvent OutroAccesible;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        if (CurrentLevelSO.Value == CurrentLevelSO.Max)
        {
            Debug.Log("TEST");
            OutroAccesible.Raise();
        }
    }
}
