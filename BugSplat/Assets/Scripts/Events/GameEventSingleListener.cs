using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventSingleListener : GameEventListener
{
    public GameObject Target;

    public override void OnEventRaised(GameObject source) {
        Debug.Log("YO");
        if (Target == source) {
            Debug.Log("OY");
            Response.Invoke(source);
        }
    }
}
