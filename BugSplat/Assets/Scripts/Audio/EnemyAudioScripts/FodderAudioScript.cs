using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FodderAudioScript : MonoBehaviour
{
    [Header("Wwise events")]
    public AK.Wwise.Event Footstep;
    public AK.Wwise.Event Attack;
    public AK.Wwise.Event Hit;
    public AK.Wwise.Event ScaredScream;
    public AK.Wwise.Event DeathSplat;
    public AK.Wwise.Event PlayerDetected;


    public void FootStepEvent(GameObject source)
    {
        Footstep.Post(source);
    }

    public void AttackEvent(GameObject source)
    {
        Attack.Post(source);
        Debug.Log("Fodder attacked you so we played an audio file");
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

    public void PlayerDetect(GameObject source)
    {
        PlayerDetected.Post(source);
    }
}
