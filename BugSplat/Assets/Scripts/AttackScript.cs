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

        //Debug.DrawLine(PlayerGraphics.position, (_nearstTarget - PlayerGraphics.position), Color.red);
        _coneHideTimer += Time.deltaTime;
        if (_coneHideTimer > 0.5f)
        {
            _cone.SetActive(false);
        }

        //LockOnToNearestTarget();
    }

    public override void LoopLateUpdate(float deltaTime)
    {


    }

    public void AttackNearestTarget()
    {
        if (PlayerSpeedDirectionSO.Value == Vector3.zero)
            PlayerSpeedDirectionSO.Value = transform.forward;


        LockOnToNearestTarget();
        if (_lockedOntoTarget)
        {
            _directionToNearstTarget *= 100;
            _directionToNearstTarget = _directionToNearstTarget.normalized;

            //print(_directionToNearstTarget);
            PlayerSpeedDirectionSO.Value.x = _directionToNearstTarget.x;
            PlayerSpeedDirectionSO.Value.z = _directionToNearstTarget.z;

            if (_distanceToNearstTarget > (AttackLength.Value * 0.5f))
                transform.Translate(PlayerSpeedDirectionSO.Value * (_distanceToNearstTarget - (AttackLength.Value * 0.5f)));


            Attack();
        }
        else
        {
            print("no targets");

            transform.Translate(PlayerSpeedDirectionSO.Value * AttackMoveDistance.Value);

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
            //Debug.Log(potentialTargets[i].name);

            Vector3 temp = potentialTargets[i].transform.position;
            temp.y = PlayerGraphics.position.y;



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

            _nearstTarget.y = PlayerGraphics.position.y;
            _distanceToNearstTarget = distance;


            _directionToNearstTarget = _nearstTarget - PlayerGraphics.position;
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

        // print(potentialTargets.Length);

        for (int i = 0; i < potentialTargets.Length; i++)
        {

            //print(Vector3.Angle(PlayerGraphics.position + transform.forward, potentialTargets[i].transform.position - PlayerGraphics.position));
            //if()
            Vector3 temp = potentialTargets[i].transform.position;
            temp.y = PlayerGraphics.position.y;

            //print(Vector3.Angle(transform.position - (transform.position + PlayerSpeedDirectionSO.Value), transform.position - temp));
            // print("angle is "+ Vector3.Angle(PlayerGraphics.position - (PlayerGraphics.position + PlayerSpeedDirectionSO.Value), PlayerGraphics.position - temp) + " " + AttackAngle.Value);

            //print(PlayerSpeedDirectionSO.Value);

            if (Vector3.Angle(PlayerGraphics.position - (PlayerGraphics.position + PlayerSpeedDirectionSO.Value), PlayerGraphics.position - temp) < AttackAngle.Value)
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
