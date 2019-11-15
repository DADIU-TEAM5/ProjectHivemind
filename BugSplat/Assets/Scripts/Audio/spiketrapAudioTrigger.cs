using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spiketrapAudioTrigger : MonoBehaviour
{

    [Header("Game events")]
    public GameEvent spikeTrapRetractEvent;
    public GameEvent spikeTrapStabEvent;
    public GameEvent spikeTrapImpactEvent;

    public void spiketrapRetractTrigger()
    {
        Debug.Log("animation event was played");
        spikeTrapRetractEvent.Raise(this.gameObject);
    }

    public void spiketrapStabTrigger()
    {
        spikeTrapStabEvent.Raise(this.gameObject);
    }

    public void spiketrapImpactTrigger()
    {
        spikeTrapImpactEvent.Raise(this.gameObject);
    }

}
