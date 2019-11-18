using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slidingDoor : MonoBehaviour
{
    [Header("Wwise events")]
    public AK.Wwise.Event slidingDoorEvent;



    public void OnDoorOpenAndClose(GameObject source)
    {
        slidingDoorEvent.Post(source);
    }


}