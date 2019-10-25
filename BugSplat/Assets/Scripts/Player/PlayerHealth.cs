using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public FloatVariable MaxHealth;

    [SerializeField]
    private FloatVariable CurrentHealth;


    [SerializeField]
    private GameEvent TookDamageEvent;

    public void Start()
    {
        CurrentHealth.Value = MaxHealth.Value;
    }

    public void TakeDamage(float damage)
    {
        print("The player took " + damage);
        CurrentHealth.Value -= damage;

        if (damage > 0f) {
            TookDamageEvent.Raise();
        }
    }
}
