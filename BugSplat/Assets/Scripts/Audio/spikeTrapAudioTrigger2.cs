using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeTrapAudioTrigger2 : MonoBehaviour
{
    [Header("Game events")]
    public GameEvent spikeTrapRetractEvent;
    public GameEvent spikeTrapStabEvent;
    public GameEvent spikeTrapImpactEvent;


    Renderer _renderer;

    private void OnEnable()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void spiketrapRetractTrigger()
    {

        if (_renderer.isVisible)
        {
            //Debug.Log("animation event was played");
            spikeTrapRetractEvent.Raise(this.gameObject);
        }

        
    }

    public void spiketrapStabTrigger()
    {

        if (_renderer.isVisible)
            spikeTrapStabEvent.Raise(this.gameObject);
    }

    public void spiketrapImpactTrigger()
    {
        if (_renderer.isVisible)
            spikeTrapImpactEvent.Raise(this.gameObject);
    }

}
