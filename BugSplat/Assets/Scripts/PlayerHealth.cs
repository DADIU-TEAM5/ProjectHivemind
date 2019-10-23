using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public FloatVariable Health;
    float _currentHealth;

    public void Start()
    {
        _currentHealth = Health.Value;
    }

    public void TakeDamage(float damage)
    {
        print("the player took " + damage);
        _currentHealth -= damage;

    }
}
