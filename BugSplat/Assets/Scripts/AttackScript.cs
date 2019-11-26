using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackScript : GameLoop
{
    
    public Animator Anim;

    public FloatVariable AttackLength;
    public FloatVariable AttackAngle;
    public FloatVariable AttackMoveDistance;
    public FloatVariable AttackDamage;
    public FloatVariable AttackCooldown;

    public Vector3Variable PlayerDirectionSO;

    public Transform PlayerGraphics;
    public GameObjectVariable LockedTarget;

    public FloatVariable AutoAttackRange;

    public GameObjectVariable CurrentEnemySO;

    public GameEvent AttackOnHit;
    public GameEvent AttackInitiated;

    NavMeshAgent _navMeshAgent;
    Vector3 _nearstTarget;
    bool _lockedOntoTarget;
    float _distanceToNearstTarget;
    Vector3 _directionToNearstTarget;

    float _coneHideTimer;

    GameObject _cone;
    LineRenderer _coneRenderer;
    Rigidbody _rigidbody;

    private bool _canAttack = true;


    float _attackingTimer;


    public FloatVariable AttackTime;
    public BoolVariable Attacking;


    // Start is called before the first frame update
    void Start()
    {

        



        _cone = new GameObject();

        _rigidbody= GetComponent<Rigidbody>();
        _cone.AddComponent<LineRenderer>();
        _coneRenderer = _cone.GetComponent<LineRenderer>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _cone.SetActive(false);

    }

    public override void LoopUpdate(float deltaTime)
    {


        if (_attackingTimer <= 0)
        {
            Attacking.Value = false;
        }
        else
        {
            _attackingTimer -= deltaTime;
        }


        //Debug.DrawLine(PlayerGraphics.position, (_nearstTarget - PlayerGraphics.position), Color.red);
        _coneHideTimer += deltaTime;
        if (_coneHideTimer > 0.5f)
        {
            _cone.SetActive(false);
        }

        LockOnToNearestTarget();
    }

    public override void LoopLateUpdate(float deltaTime)
    {


    }

    public void AttackNearestTarget()
    {


        if (!_canAttack) return;

        Attacking.Value = true;
        _attackingTimer = AttackTime.Value;
        StartCoroutine(StartAttackCooldown());

        PlayerDirectionSO.Value =  PlayerGraphics.forward;

        LockOnToNearestTarget();
        if (_lockedOntoTarget)
        {
            _directionToNearstTarget = _directionToNearstTarget.normalized;

            //print(_directionToNearstTarget);
            PlayerDirectionSO.Value.x = _directionToNearstTarget.x;
            PlayerDirectionSO.Value.z = _directionToNearstTarget.z;

            if (_distanceToNearstTarget > (AttackLength.Value * 0.5f))
            {
                if (_distanceToNearstTarget > AttackMoveDistance.Value + (AttackLength.Value * 0.5f))
                {
                    _navMeshAgent.Move(PlayerDirectionSO.Value * (AttackMoveDistance.Value ));
                }
                else
                {
                    _navMeshAgent.Move(PlayerDirectionSO.Value * (_distanceToNearstTarget - (AttackLength.Value * 0.5f)));
                }
            }

            Attack();
        }
        else
        {
            RaycastHit hit;
            if(Physics.CapsuleCast(transform.position - (Vector3.up * 0.5f), transform.position + (Vector3.up * 0.5f), .1f, PlayerDirectionSO.Value, out hit))
            {
                float ditanceToObject = Vector3.Distance(hit.point, transform.position);
                //print(hit.collider.gameObject.name);
                if (ditanceToObject > AttackMoveDistance.Value)
                {
                    transform.Translate(PlayerDirectionSO.Value * AttackMoveDistance.Value);
                }
                else
                {
                    transform.Translate(PlayerDirectionSO.Value * ditanceToObject);
                }
            }
            else
            {
                transform.Translate(PlayerDirectionSO.Value * AttackMoveDistance.Value);
            }

            Attack();
        }
    }


    private void LockOnToNearestTarget()
    {
        
            Collider[] potentialTargets = Physics.OverlapSphere(PlayerGraphics.position, AutoAttackRange.Value, LayerMask.GetMask("Enemy"));

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

                CurrentEnemySO.Value = potentialTargets[targetIndex].gameObject;
            } 
            else
            {
                _lockedOntoTarget = false;
                _nearstTarget = Vector3.zero;
                CurrentEnemySO.Value = null;
            }

        /*if (LockedTarget.Value != null)
        {
            _lockedOntoTarget = true;
            _nearstTarget = LockedTarget.Value.transform.position;

            _nearstTarget.y = PlayerGraphics.position.y;
            _distanceToNearstTarget = Vector3.Distance(PlayerGraphics.position, _nearstTarget);


            _directionToNearstTarget = _nearstTarget - PlayerGraphics.position;
        }*/
    }

    private IEnumerator StartAttackCooldown()
    {
        _canAttack = false;

        yield return new WaitForSeconds(AttackCooldown.Value);

        _canAttack = true;
    }

    
    private void Attack()
    {
        //print(PlayerDirectionSO.Value);

        PlayerGraphics.rotation = Quaternion.LookRotation(PlayerDirectionSO.Value, PlayerGraphics.up);
        AttackInitiated.Raise(PlayerGraphics.gameObject);
        Anim.SetTrigger("Attack");

        DrawCone(10);
        _cone.SetActive(true);
        _coneHideTimer = 0;

        Collider[] potentialTargets = Physics.OverlapSphere(PlayerGraphics.position, AttackLength.Value, LayerMask.GetMask("Enemy"));

        // print(potentialTargets.Length);
        int layer = 1 << 9;
        layer = ~layer;

        List<GameObject> Targets = new List<GameObject>();

        for (int i = 0; i < potentialTargets.Length; i++)
        {
            

            RaycastHit[] Hits = Physics.RaycastAll(PlayerGraphics.position, potentialTargets[i].transform.position - transform.position, AttackLength.Value, layer);

            float Distance = float.MaxValue;

            for (int k = 0; k < Hits.Length; k++)
            {
                if(Hits[k].collider.gameObject.layer != 8)
                {
                    if(Hits[k].distance < Distance)
                    {
                        Distance = Hits[k].distance;
                    }
                }
            }
            for (int k = 0; k < Hits.Length; k++)
            {
                if (Hits[k].collider.gameObject.layer == 8)
                {
                    if (Hits[k].distance < Distance)
                    {
                        if(!Targets.Contains(Hits[k].collider.gameObject))
                        Targets.Add(Hits[k].collider.gameObject);
                    }
                }
            }



            
            
        }
        for (int i = 0; i < Targets.Count; i++)
        {
            
                
                    Vector3 temp = Targets[i].transform.position;
                    temp.y = PlayerGraphics.position.y;

                    

                    if (Vector3.Angle(PlayerGraphics.position - (PlayerGraphics.position + PlayerDirectionSO.Value), PlayerGraphics.position - temp) < AttackAngle.Value)
                    {
                        var potentialEnemy = Targets[i].GetComponent<Enemy>();

                        AttackOnHit.Raise(potentialEnemy.gameObject);
                        potentialEnemy.TakeDamage(AttackDamage.Value);
                    }
                

            
        }
        


    }

    void DrawCone(int points)
    {
        Vector3[] pointsForTheCone = new Vector3[points];
        _coneRenderer.positionCount = points;

        pointsForTheCone[0] = PlayerGraphics.position;

        Vector3 vectorToRotate = PlayerDirectionSO.Value * AttackLength.Value;
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

