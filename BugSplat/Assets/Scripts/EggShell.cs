using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggShell : MonoBehaviour
{
    public GameObject broken;
    public GameObject whole;
    public GameObject BabyToSpawn;

    public float Damage;

    bool hit = false;

    public Rigidbody Body;

    

    public void SetLifeTime(float time)
    {
        StartCoroutine(timedDeath(time));
    }

    public void setSpeed(float speed)
    {
        Body.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hit)
        {
            if(collision.gameObject.layer == 9)
            {

                collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(Damage);

            }

            hit = true;
            BreakEgg();
            
        }
    }

    public void BreakEgg()
    {

        whole.SetActive(false);
        broken.SetActive(true);

        GameObject baby = Instantiate(BabyToSpawn);

        baby.GetComponent<Enemy>().SpawnedEnemy = true;

        Vector3 spawnPoint = transform.position;

        spawnPoint.y = 0;

        baby.transform.position = spawnPoint;

        Destroy(gameObject, 2);
    }

    IEnumerator timedDeath(float time)
    {
        yield return new WaitForSeconds(time);

        if(hit == false)
        BreakEgg();

        yield return null;
    }

}
