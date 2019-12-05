using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EventCollider : MonoBehaviour
{
    public GameEvent EnterEvent;
    public GameEvent ExitEvent;
    public GameEvent StayEvent;

    bool exitTriggered;
    bool enteredTriggered;

    private void OnEnable()
    {
        exitTriggered = false;
        enteredTriggered = false;
    }

    void OnTriggerEnter(Collider other) {

        if (!enteredTriggered)
        {
            EnterEvent?.Raise(this.gameObject);
            enteredTriggered = true;
        }
    }

    void OnTriggerExit(Collider other) {

        if (!exitTriggered)
        {
            ExitEvent?.Raise(this.gameObject);
            exitTriggered = true;
        }

        
    }

    void OnTriggerStay(Collider other) {
        StayEvent?.Raise(this.gameObject);
    }
}
