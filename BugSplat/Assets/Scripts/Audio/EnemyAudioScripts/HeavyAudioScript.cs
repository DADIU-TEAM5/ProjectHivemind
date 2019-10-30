using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAudioScript : MonoBehaviour
{
    public AK.Wwise.Event Footstep;
    public AK.Wwise.Event Attack;
    public AK.Wwise.Event Hit;
    public AK.Wwise.Event ScaredScream;
    public AK.Wwise.Event DeathSplat;
    public AK.Wwise.Event Charge;

    // Skal bruges til voldsommere footsteps, måske skal det ske igennem events, eg. charge event sætter chargestate,
    //og hit eller time run out sets state back
    // private AK.Wwise.State ChargingState;


    public void FootStepEvent(GameObject source)
    {
        Footstep.Post(source);
    }

    public void AttackEvent(GameObject source)
    {
        Attack.Post(source);
    }

    public void HitEvent(GameObject source)
    {
        Hit.Post(source);
    }

    public void Scared(GameObject source)
    {
        ScaredScream.Post(source);
    }

    public void Death(GameObject source)
    {
        DeathSplat.Post(source);
    }

    public void ChargingEvent()
    {
        Charge.Post(this.gameObject);
    }
}
