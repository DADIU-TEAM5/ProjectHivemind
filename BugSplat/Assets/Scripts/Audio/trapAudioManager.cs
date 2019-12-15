using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapAudioManager : MonoBehaviour
{

    [Header("Spiketrap Wwise events")]
    public AK.Wwise.Event spiketrapStab;
    public AK.Wwise.Event spiketrapImpact;
    public AK.Wwise.Event spiketrapRetract;

    public void spiketrapStabEvent(GameObject source)
    {
        spiketrapStab.Post(source);
    }

    public void spiketrapImpactEvent(GameObject source)
    {
        spiketrapImpact.Post(source);
    }

    public void spiketrapRetractEvent(GameObject source)
    {
        spiketrapRetract.Post(source);
    }
}
