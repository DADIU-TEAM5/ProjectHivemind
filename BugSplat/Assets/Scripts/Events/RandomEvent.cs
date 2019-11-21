using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEvent : MonoBehaviour
{
    public GameEvent[] Events;

    public void RaiseRandom() {
        if (Events == null || Events.Length == 0) return;

        Debug.Log(Events.Length);
        var randomIndex = Random.Range(0, Events.Length);
        Debug.Log(randomIndex);

        Events[randomIndex]?.Raise();
    }
}
