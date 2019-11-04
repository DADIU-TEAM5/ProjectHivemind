using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Apply Debuff")]
public class ApplyDebuffAbility : Ability
{
    public Ability Debuff;
    public GameObject DebuffPrefab;
    public GameEvent EnemyHit;
    public Vector3 DebuffOffset;

    public override void Initialize(GameObject GameObj)
    {
    }

    public override void OnTrigger(GameObject Enemy)
    {
        if (Enemy != null)
        {
            Vector3 debuffPos = Enemy.transform.position;

            debuffPos += DebuffOffset;

            GameObject _debuff = Instantiate(DebuffPrefab, debuffPos ,Quaternion.identity,Enemy.transform);
                       
        }

    }

}
