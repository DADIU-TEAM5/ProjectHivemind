using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitterAudioScript : MonoBehaviour
{
    public AK.Wwise.Event Footstep;
    public AK.Wwise.Event Attack;
    public AK.Wwise.Event Hit;
    public AK.Wwise.Event ScaredScream;
    public AK.Wwise.Event DeathSplat;
    public AK.Wwise.Event RangedAttack;
    public AK.Wwise.Event EmergeFromGround;
    public AK.Wwise.Event HideUnderground;


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

    public void RangedAttackEvent(GameObject source)
    {
        RangedAttack.Post(source);
    }

    public void EmergeEvent(GameObject source)
    {
        EmergeFromGround.Post(source);
    }

    public void HideEvent(GameObject source)
    {
        HideUnderground.Post(source);
    }
}
