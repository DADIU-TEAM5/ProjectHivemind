using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAudioScript : MonoBehaviour
{
    public AK.Wwise.Event Footstep;
    public AK.Wwise.Event Attack;
    public AK.Wwise.Event HitFront;
    public AK.Wwise.Event HitBack;
    public AK.Wwise.Event Detect;
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

    public void HitFrontEvent(GameObject source)
    {
        HitFront.Post(source);
    }

    public void HitBackEvent(GameObject source)
    {
        HitBack.Post(source);
    }

    public void DetectEvent(GameObject source)
    {
        Detect.Post(source);
    }

    public void DeathEvent(GameObject source)
    {
        DeathSplat.Post(source);
    }

    public void ChargingEvent()
    {
        Charge.Post(this.gameObject);
    }
}
