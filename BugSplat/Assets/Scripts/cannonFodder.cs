using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannonFodder : Enemy
{
    bool _playerDetected;
    public SimpleEnemyStats stats;
    Transform _playerTransform;
    bool _attacking;
    float _attackCharge;

    public override void TakeDamage(float damage)
    {
        print(name + " took damage "+ damage);
    }


    public override void LoopUpdate(float deltaTime)
    {
        if (!_playerDetected)
        {
            DetectThePlayer();
        }
        else if(playerInAttackRange() || _attacking)
        {
            Attack();
        }
        else
        {
            MoveTowardsThePlayer();
        }



    }
    public override void LoopLateUpdate(float deltaTime)
    {

    }

    void Attack()
    {
        _attacking = true;
        _attackCharge += Time.deltaTime;

        if (_attackCharge >= stats.AttackChargeUpTime)
        {

            Collider[] potentialTargets = Physics.OverlapSphere(transform.position, stats.AttackRange, LayerMask.GetMask("Player"));

            for (int i = 0; i < potentialTargets.Length; i++)
            {

                //print(Vector3.Angle(transform.position + transform.forward, potentialTargets[i].transform.position - transform.position));
                //if()
                Vector3 temp = potentialTargets[i].transform.position;
                temp.y = transform.position.y;


                if (Vector3.Angle(transform.position - (transform.position + transform.forward), transform.position - temp) < stats.AttackAngle)
                {
                    //apply damage to the player
                    if (potentialTargets[i].GetComponent<PlayerHealth>() != null)
                    {

                        potentialTargets[i].GetComponent<PlayerHealth>().TakeDamage(stats.AttackDamage);
                    }
                    else
                    {
                        print("target got no health");
                    }
                }




            }
            _attacking = false;
            _attackCharge = 0;
        }

    }

    bool playerInAttackRange()
    {
        
        Vector3 adjustedPlayerPos = _playerTransform.position;

        adjustedPlayerPos.y = transform.position.y;

        return Vector3.Distance(transform.position, adjustedPlayerPos) < stats.AttackRange / 2;
    }

    void MoveTowardsThePlayer()
    {
        Vector3 adjustedPlayerPos = _playerTransform.position;

        adjustedPlayerPos.y = transform.position.y;

        transform.LookAt(adjustedPlayerPos);

        transform.Translate(Vector3.forward * stats.MoveSpeed * Time.deltaTime);

    }

    void DetectThePlayer()
    {
        Collider[] potentialTargets = Physics.OverlapSphere(transform.position, stats.SpotDistance, LayerMask.GetMask("Player"));

        if(potentialTargets.Length > 0)
        {
            _playerDetected = true;
            _playerTransform =  potentialTargets[0].gameObject.transform;
        } 
    }
}
