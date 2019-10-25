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

    float _coneHideTimer;

    GameObject _cone;
    LineRenderer _coneRenderer;


    // Start is called before the first frame update
    void Start()
    {

        _cone = new GameObject();


        _cone.AddComponent<LineRenderer>();
        _coneRenderer = _cone.GetComponent<LineRenderer>();

        _cone.SetActive(false);


    }

    public override void LoopUpdate(float deltaTime)
    {
        _coneHideTimer += Time.deltaTime;
        if(_coneHideTimer > 0.5f)
        {
            _cone.SetActive ( false);
        }
        

        /*Debug.DrawLine(transform.position, transform.position + transform.forward, Color.yellow);
        if (Input.GetButtonDown("Jump"))
        {
            AttackNearestTarget();
        }*/
       

        
    }

    public override void LoopLateUpdate(float deltaTime)
    {
       

    }

    public void AttackNearestTarget()
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
        drawCone(10);
        _cone.SetActive(true);
        _coneHideTimer = 0;

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

    void drawCone(int points)
    {
        Vector3[] pointsForTheCone = new Vector3[points];
        _coneRenderer.positionCount = points;

        pointsForTheCone[0] = transform.position;

        Vector3 vectorToRotate = transform.forward * AttackLength.Value;
        Vector3 rotatedVector = Vector3.zero;

        float stepSize = 1f / ((float)points - 1);
        int step = 0;

        for (int i = 1; i < points; i++)
        {
            float angle = Mathf.Lerp(-AttackAngle.Value, AttackAngle.Value, step * stepSize);



            angle = angle * Mathf.Deg2Rad;

            float s = Mathf.Sin(angle);
            float c = Mathf.Cos(angle);

            rotatedVector.x = vectorToRotate.x * c - vectorToRotate.z * s;
            rotatedVector.z = vectorToRotate.x * s + vectorToRotate.z * c;

            pointsForTheCone[i] = transform.position + rotatedVector;
            step++;
        }





        _coneRenderer.SetPositions(pointsForTheCone);
        _coneRenderer.widthMultiplier = 0.1f;

        _coneRenderer.loop = true;
    }

}
