using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cageAudioPlayer : MonoBehaviour
{
    public AK.Wwise.Event cageStart;
    public AK.Wwise.Event cageStop;
    public AK.Wwise.Event cageOpen;


    public void CageStartEvent(GameObject source)
    {
        cageStart.Post(source);
    }

    public void CageStopEvent(GameObject source)
    {
        cageStop.Post(source);
    }

    public void CageOpenEvent(GameObject source)
    {
        cageOpen.Post(source);
    }
}
