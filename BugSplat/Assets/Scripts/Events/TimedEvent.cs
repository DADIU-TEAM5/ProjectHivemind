using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedEvent : MonoBehaviour
{
    public float Time;

    public GameEvent Event;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(Time);

        Event.Raise();
    }
}
