using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitProjectile : MonoBehaviour
{
    public SpitterStats stats;


    private void OnParticleCollision(GameObject other)
    {

        print(other.name);
        if(other.CompareTag("Player"))
        {

            other.transform.GetChild(0). GetComponent<PlayerHealth>().TakeDamage(stats.AttackDamage);
        }
    }
}
