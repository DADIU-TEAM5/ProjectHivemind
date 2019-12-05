using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EventCollider : MonoBehaviour
{
    public GameEvent EnterEvent;
    public GameEvent ExitEvent;
    public GameEvent StayEvent;

    bool hasTriggered;

    private void OnEnable()
    {
        hasTriggered = false;
    }

    void OnTriggerEnter(Collider other) {
        if (!hasTriggered)
        {
            EnterEvent?.Raise(this.gameObject);
            hasTriggered = true;
        }


    }

    void OnTriggerExit(Collider other) {
        if (!hasTriggered)
        {
            EnterEvent?.Raise(this.gameObject);
            hasTriggered = true;
        }
    }

    void OnTriggerStay(Collider other) {
        StayEvent?.Raise(this.gameObject);
    }
}
