using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerAudioScript : MonoBehaviour
{
    public AK.Wwise.Event FootStep;
    public AK.Wwise.Event Attack;
    public AK.Wwise.Event Hit; 
    public AK.Wwise.Event ScaredScream;
    public AK.Wwise.Event DeathSplat;
    public AK.Wwise.Event BoomerAttack;
    public AK.Wwise.Event Detect;


    public void FootstepEvent(GameObject source)
    {
        FootStep.Post(source);
    }

    public void DetectEvent(GameObject source)
    {
        Detect.Post(source);
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

    public void BoomerAttackEvent(GameObject source)
    {
        BoomerAttack.Post(source);
    }
}
