using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : GameLoop
{
    public FloatVariable AttackLength;
    public FloatVariable AttackAngle;
    public FloatVariable AttackMoveDistance;
    public FloatVariable AttackDamage;
    public FloatVariable AttackCooldown;

    Vector3 _nearstTarget;
    bool _lockedOntoTarget;
    float _distanceToNearstTarget;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void LoopUpdate(float deltaTime)
    {

        

        Debug.DrawLine(transform.position, transform.position + transform.forward, Color.yellow);
        if (Input.GetButtonDown("Jump"))
        {
            AttackNearestTarget();
        }
       

        
    }

    public override void LoopLateUpdate(float deltaTime)
    {
       

    }

    private void AttackNearestTarget()
    {
        
        
        if (_lockedOntoTarget)
        {


            transform.LookAt(_nearstTarget);

            if (_distanceToNearstTarget > AttackLength.Value)
                transform.Translate(Vector3.forward * (_distanceToNearstTarget - AttackLength.Value));

            Attack();
        }
        else
        {
            print("no targets");

            transform.Translate(Vector3.forward * AttackLength.Value);
            Attack();

        }


    }


    private void LockOnToNearestTarget()
    {
        Collider[] potentialTargets = Physics.OverlapSphere(transform.position, AttackLength.Value + AttackMoveDistance.Value, LayerMask.GetMask("Enemy"));

        int targetIndex = -1;
        float distance = float.MaxValue;



        for (int i = 0; i < potentialTargets.Length; i++)
        {
            Vector3 temp = potentialTargets[i].transform.position;
            temp.y = transform.position.y;
            float newDistance = Vector3.Distance(transform.position, temp);

            

            if (newDistance < distance)
            {
                distance = newDistance;
                targetIndex = i;

            }
        }
        if (potentialTargets.Length >0)
        {
            _lockedOntoTarget = true;
            _nearstTarget = potentialTargets[targetIndex].transform.position;
            transform.LookAt(_nearstTarget);
            _distanceToNearstTarget = distance;
        }
        else
        {
            _lockedOntoTarget = false;
        }
        
    }

    private void Attack()
    {
        Collider[] potentialTargets = Physics.OverlapSphere(transform.position, AttackLength.Value, LayerMask.GetMask("Enemy"));

        for (int i = 0; i < potentialTargets.Length; i++)
        {

            //print(Vector3.Angle(transform.position + transform.forward, potentialTargets[i].transform.position - transform.position));
            //if()
            Vector3 temp = potentialTargets[i].transform.position;
            temp.y = transform.position.y;

            
            if (Vector3.Angle(transform.position - (transform.position + transform.forward), transform.position - temp) < AttackAngle.Value)
                potentialTargets[i].GetComponent<Enemy>().TakeDamage(AttackDamage.Value);
            


        }

    }

}
