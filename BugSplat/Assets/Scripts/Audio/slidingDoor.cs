using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slidingDoor : MonoBehaviour
{
    [Header("Wwise events")]
    public AK.Wwise.Event slidingDoorEvent;
    public AK.Wwise.Event arenaGate;



    public void OnDoorOpenAndClose(GameObject source)
    {
        slidingDoorEvent.Post(source);
    }

    public void ArenaGateOpen(GameObject source)
    {
        arenaGate.Post(source);
    }
}