using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FodderAudioScript : MonoBehaviour
{
    public AK.Wwise.Event FootStep;
    public AK.Wwise.Event Attack;
    public AK.Wwise.Event Hit;
    public AK.Wwise.Event ScaredScream;
    public AK.Wwise.Event DeathSplat;


    public void FootStepEvent()
    {
        FootStep.Post(this.gameObject);
    }

    public void AttackEvent()
    {
        Attack.Post(this.gameObject);
    }

    public void HitStepEvent()
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
