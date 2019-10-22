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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void LoopUpdate(float deltaTime)
    {

        ChooseTarget();

        Debug.DrawLine(transform.position, transform.position + transform.forward, Color.yellow);
        if (Input.GetButtonDown("Jump"))
        {
            Attack();
        }
       

        
    }

    public override void LoopLateUpdate(float deltaTime)
    {
       

    }

    public void ChooseTarget()
    {
        
        Collider[] potentialTargets = Physics.OverlapSphere(transform.position, AttackLength.Value+AttackMoveDistance.Value, LayerMask.GetMask("Enemy"));

        int targetIndex = -1;
        float distance =float.MaxValue;

        

        for (int i = 0; i < potentialTargets.Length; i++)
        {
            Vector3 temp = potentialTargets[i].transform.position;
            temp.y = transform.position.y;
            float newDistance = Vector3.Distance(transform.position, temp);


            if ( newDistance < distance)
            {
                distance = newDistance;
                targetIndex = i;
                
            }
        }
        if(targetIndex != -1)
        {
            Vector3 temp = potentialTargets[targetIndex].transform.position;
            temp.y = transform.position.y;

            transform.LookAt(temp);
        }


    }

    public void Attack()
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
