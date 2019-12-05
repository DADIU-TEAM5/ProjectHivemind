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

        print(gameObject.name + " was entered");
        if (!enteredTriggered)
        {
            EnterEvent?.Raise(this.gameObject);
            enteredTriggered = true;
        }

        EnterEvent?.Raise(this.gameObject);


    }

    void OnTriggerExit(Collider other) {

        print(gameObject.name + " was exited");
        if (!exitTriggered)
        {
            print("for the first time");

            ExitEvent?.Raise(this.gameObject);
            exitTriggered = true;
        }

        
    }

    void OnTriggerStay(Collider other) {
        StayEvent?.Raise(this.gameObject);
    }
}
