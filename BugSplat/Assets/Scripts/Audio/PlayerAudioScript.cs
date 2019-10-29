using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioScript : MonoBehaviour
{
    [Header("Wwise events")]
    public AK.Wwise.Event Footstep;
    public AK.Wwise.Event Attack;
    public AK.Wwise.Event Dash;
    public AK.Wwise.Event Hit;
    public AK.Wwise.Event ScaredScream;
    public AK.Wwise.Event DeathSplat;

    [Header("Wwise RTPCs")]
    public AK.Wwise.RTPC PlayerSpeedRTPC;


    [Header("Variables")]
    public Vector3Variable PlayerSpeedVector3;

    /*[Header("GameEvents")]
    public UnityEngine.Events.UnityEvent[] Event;
    */

    void Update()
    {
        //PH
        PlayerSpeedRTPC.SetValue(this.gameObject, PlayerSpeedVector3.Value.magnitude);
    }

    public void FootStepEvent()
    {
        Footstep.Post(this.gameObject);
    }

    public void AttackEvent()
    {
        Attack.Post(this.gameObject);
    }

    public void DashEvent()
    {
        Dash.Post(this.gameObject);
    }

    public void HitEvent()
    {
        Hit.Post(this.gameObject);
    }

    public void Scared()
    {
        ScaredScream.Post(this.gameObject);
    }

    public void Death()
    {
        DeathSplat.Post(this.gameObject);
    }
}
