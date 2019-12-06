using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeTrapAudioTrigger2 : MonoBehaviour
{
    [Header("Game events")]
    public GameEvent spikeTrapRetractEvent;
    public GameEvent spikeTrapStabEvent;
    public GameEvent spikeTrapImpactEvent;

    public LayerMask mask;

    Renderer _renderer;

    private void OnEnable()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void spiketrapRetractTrigger()
    {

        if (_renderer.isVisible)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, 5, mask);

            if(hits.Length > 0)
            {
                spikeTrapRetractEvent.Raise(this.gameObject);
            }

           
        }

        
    }

    public void spiketrapStabTrigger()
    {

        if (_renderer.isVisible)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, 5, mask);

            if (hits.Length > 0)
            {
                spikeTrapStabEvent.Raise(this.gameObject);
            }

        }


            
    }

    public void spiketrapImpactTrigger()
    {
        if (_renderer.isVisible)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, 5, mask);

            if (hits.Length > 0)
            {
                spikeTrapImpactEvent.Raise(this.gameObject);
            }

        }
            
    }

}
