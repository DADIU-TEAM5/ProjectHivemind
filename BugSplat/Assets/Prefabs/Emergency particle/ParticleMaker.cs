using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMaker : MonoBehaviour
{

    public GameObject[] ParticlesToSpawn;

    public void SpawnParticles()
    {

        for (int i = 0; i < ParticlesToSpawn.Length; i++)
        {
           GameObject particle =  Instantiate(ParticlesToSpawn[i], transform);

            Destroy(particle, 0.5f);
        }

    }


}
