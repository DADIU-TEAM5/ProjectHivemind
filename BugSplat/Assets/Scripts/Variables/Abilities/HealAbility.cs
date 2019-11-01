using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Heal Ability")]
public class HealAbility : Ability
{
    public float HealValue;
    public FloatVariable CurrentHealth;
    public FloatVariable MaxHealth;

    public override void Initialize(GameObject GameObj)
    {
    }

    public override void OnTrigger()
    {
        Debug.Log("Heal Ability is Triggering");
        CurrentHealth.Value = Mathf.Min(CurrentHealth.Value + HealValue, MaxHealth.Value);
    }

}
