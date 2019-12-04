using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toadSpeak : MonoBehaviour
{
    public GameEvent ToadSpeakTrigger;
    public GameEvent ToadEatTrigger;

    public void ToadSpeakEvent()
    {
        ToadSpeakTrigger.Raise();
    }

    public void ToadEat()
    {
        ToadEatTrigger.Raise();
    }
}
