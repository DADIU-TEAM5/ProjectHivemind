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

    public Vector3Variable PlayerSpeedDirectionSO;
    public FloatVariable PlayerMaxSpeedSO;

    public Transform PlayerGraphics;

    Vector3 _nearstTarget;
    bool _lockedOntoTarget;
    float _distanceToNearstTarget;
    Vector3 _directionToNearstTarget;

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

        LockOnToNearestTarget();
    }

    public override void LoopLateUpdate(float deltaTime)
    {
       

    }

    public void AttackNearestTarget()
    {

        if (_lockedOntoTarget)
        {
            if (_distanceToNearstTarget > AttackLength.Value)
            {
                // Export direction and speed vector to the PlayerSpeedDirectionSO
                PlayerSpeedDirectionSO.Value.x = _directionToNearstTarget.x;
                PlayerSpeedDirectionSO.Value.z = _directionToNearstTarget.z;

                transform.Translate(PlayerSpeedDirectionSO.Value * (_distanceToNearstTarget - AttackLength.Value));
            }

            Attack();
        }
        else
        {
            print("no targets");

            transform.Translate(PlayerSpeedDirectionSO.Value * AttackLength.Value);

            Attack();

        }


    }


    private void LockOnToNearestTarget()
    {
        Collider[] potentialTargets = Physics.OverlapSphere(PlayerGraphics.position, AttackLength.Value + AttackMoveDistance.Value, LayerMask.GetMask("Enemy"));

        int targetIndex = -1;
        float distance = float.MaxValue;

        for (int i = 0; i < potentialTargets.Length; i++)
        {
            Debug.Log(potentialTargets[i].name);

            Vector3 temp = potentialTargets[i].transform.position;
            temp.y = PlayerSpeedDirectionSO.Value.y;
            float newDistance = Vector3.Distance(PlayerGraphics.position, temp);

            if (newDistance < distance)
            {
                distance = newDistance;
                targetIndex = i;
            }
        }
        if (potentialTargets.Length > 0)
        {
            _lockedOntoTarget = true;
            _nearstTarget = potentialTargets[targetIndex].transform.position;
            _distanceToNearstTarget = distance;
 

            _directionToNearstTarget = (_nearstTarget - PlayerGraphics.position) / _distanceToNearstTarget;
        }
        else
        {
            _lockedOntoTarget = false;
            _nearstTarget = Vector3.zero;
        }
    }

    private void Attack()
    {
        DrawCone(10);
        _cone.SetActive(true);
        _coneHideTimer = 0;

        Collider[] potentialTargets = Physics.OverlapSphere(PlayerGraphics.position, AttackLength.Value, LayerMask.GetMask("Enemy"));

        for (int i = 0; i < potentialTargets.Length; i++)
        {

            //print(Vector3.Angle(PlayerGraphics.position + transform.forward, potentialTargets[i].transform.position - PlayerGraphics.position));
            //if()
            Vector3 temp = potentialTargets[i].transform.position;
            temp.y = PlayerSpeedDirectionSO.Value.y;


            if (Vector3.Angle(PlayerGraphics.position - (PlayerGraphics.position + transform.forward), PlayerGraphics.position - temp) < AttackAngle.Value)
                potentialTargets[i].GetComponent<Enemy>().TakeDamage(AttackDamage.Value);
            
        }

    }

    void DrawCone(int points)
    {
        Vector3[] pointsForTheCone = new Vector3[points];
        _coneRenderer.positionCount = points;

        pointsForTheCone[0] = PlayerGraphics.position;

        Vector3 vectorToRotate = PlayerSpeedDirectionSO.Value * AttackLength.Value;
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

            pointsForTheCone[i] = PlayerGraphics.position + rotatedVector;
            step++;
        }





        _coneRenderer.SetPositions(pointsForTheCone);
        _coneRenderer.widthMultiplier = 0.1f;

        _coneRenderer.loop = true;
    }

}
