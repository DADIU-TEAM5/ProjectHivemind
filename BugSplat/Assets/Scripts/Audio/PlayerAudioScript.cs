using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioScript : MonoBehaviour
{
    [Header("Placeholder events")]
    public AK.Wwise.Event PhFootsteps;

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


    void Start()
    {
         
    }


    void Update()
    {
        //PH
       // PlayerSpeedRTPC.SetValue(this.gameObject, PlayerSpeedVector3.Value.magnitude);
    }

    public void FootStepEvent(GameObject source)
    {
        Footstep.Post(source);
    }

    public void AttackEvent(GameObject source)
    {
        Debug.Log(source);
        Attack.Post(source);
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
        DeathSplat.Post(source);
    }
}
