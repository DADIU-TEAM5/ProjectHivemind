using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerAudioScript : MonoBehaviour
{
   
    [Header("Wwise events")]
    public AK.Wwise.Event Footstep;
    public AK.Wwise.Event Attack;
    public AK.Wwise.Event Dash;
    public AK.Wwise.Event Hit;
    public AK.Wwise.Event ScaredScream;
    public AK.Wwise.Event Killed;
    public AK.Wwise.Event pickup;
    public AK.Wwise.Event noDash;
    public AK.Wwise.Event rangedAttack;
    public AK.Wwise.Event dashAcid;

    public void FootStepEvent(GameObject source)
    {
        Footstep.Post(source);
    }

    public void AttackEvent(GameObject source)
    {
        Attack.Post(source);
    }

    public void PickupEvent(GameObject source)
    {
        pickup.Post(source);
    }

    public void DashEvent(GameObject source)
    {
        Dash.Post(source);
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
        Killed.Post(source);
    }

    public void NoDashEvent(GameObject source)
    {
        noDash.Post(source);
    }

    public void dashAcidEvent(GameObject source)
    {
        dashAcid.Post(source);
    }

    public void rangedAttackEvent(GameObject source)
    {
        rangedAttack.Post(source);
    }
}
