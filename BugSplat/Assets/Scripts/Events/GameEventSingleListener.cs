using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventSingleListener : GameEventListener
{
    public GameObject Target;

    public override void OnEventRaised(GameObject source) {
        if (Target == source) {
            Response.Invoke(source);
        }
    }
}
