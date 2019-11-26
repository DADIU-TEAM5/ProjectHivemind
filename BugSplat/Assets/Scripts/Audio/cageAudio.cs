using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cageAudio : MonoBehaviour
{
    public GameEvent cageStart;
    public GameEvent cageStop;
    public GameEvent cageOpen;


    public void cageStartEvent()
    {
        cageStart.Raise(this.gameObject);
    }

    public void cageStopEvent()
    {
        cageStop.Raise(this.gameObject);
    }

    public void cageOpenEvent()
    {
        cageOpen.Raise(this.gameObject);
    }
}
