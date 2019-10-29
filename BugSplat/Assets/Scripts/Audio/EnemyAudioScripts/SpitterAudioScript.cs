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



    public void FootStepEvent()
    {
        Footstep.Post(this.gameObject);
    }

    public void AttackEvent()
    {
        Attack.Post(this.gameObject);
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

    public void RangedAttackEvent()
    {
        RangedAttack.Post(this.gameObject);
    }

    public void EmergeEvent()
    {
        EmergeFromGround.Post(this.gameObject);
    }

    public void HideEvent()
    {
        HideUnderground.Post(this.gameObject);
    }
}
