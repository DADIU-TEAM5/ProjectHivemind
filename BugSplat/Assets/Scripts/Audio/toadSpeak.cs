using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toadSpeak : MonoBehaviour
{
    public GameEvent ToadSpeakTrigger;

    public void ToadSpeakEvent()
    {
        ToadSpeakTrigger.Raise();
    }
}
