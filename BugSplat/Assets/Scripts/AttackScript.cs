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
    
    

    private bool _canAttack = true;


    float _attackingTimer;


    public FloatVariable AttackTime;
    public BoolVariable Attacking;


    public Material AttackMaterial;
    [Range(0,5)]
    public float ConeShowTime = 0.5f;
    [Range(0, 10)]
    public float fadeTime = 0.5f;
    [Range(0, 1)]
    public float MaxAlpha = 0.35f;

    float alpha = 0;
    bool increaseAplha = false;
    Color col;

    Mesh ConeMesh;
    MeshRenderer _coneRenderer;

    // Start is called before the first frame update
    void Start()
    {





        
        _navMeshAgent = GetComponent<NavMeshAgent>();

        CreateCone();
        col = _coneRenderer.material.color;


    }

    public override void LoopUpdate(float deltaTime)
    {
        col.a = alpha;
        _coneRenderer.material.color = col;

        if(increaseAplha && alpha < MaxAlpha)
        {
            alpha += (deltaTime * fadeTime);
        }
        else if (alpha >0)
        {

            alpha -= (deltaTime * fadeTime);
        }

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
        if (_coneHideTimer > ConeShowTime)
        {

            increaseAplha = false;
            
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
       
        increaseAplha = true;


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

    int[] _triangles = { };
    Vector3[] _normals = { };
    Vector2[] _uvs = { };

    public void DrawCone(int points)
    {
        int zeroes = points;

        points = points + zeroes;

        // zeroes -= 1;

        if (_triangles.Length != points * 3 + 3)
        {
            _triangles = new int[points * 3 + 3];

            int triangleIndex = 0;

            for (int i = 0; i < points - zeroes; i++)
            {
                if (i != points - zeroes - 1)
                {
                    _triangles[triangleIndex] = i;

                    _triangles[triangleIndex + 2] = i + zeroes;
                    _triangles[triangleIndex + 1] = i + 1 + zeroes;
                }

                triangleIndex += 3;
            }

            _triangles[triangleIndex] = zeroes;

            _triangles[triangleIndex + 2] = points - 1;
            _triangles[triangleIndex + 1] = 1;

        }

        if (_normals.Length != points)
        {

            _normals = new Vector3[points];

            for (int i = 0; i < points; i++)
            {
                _normals[i] = Vector3.up;
            }
        }


        Vector3[] vertices = new Vector3[points];
        _uvs = new Vector2[points];


        //vertices[0] = Vector3.zero;
        //_uvs[0] = new Vector2(0, 0);




        Vector3 vectorToRotate;

            vectorToRotate = Vector3.forward * AttackLength.Value;
       

        Vector3 rotatedVector = Vector3.zero;

        float stepSize = 1f / ((float)points - 2 - zeroes);

        float stepSize2 = 1f / ((float)points - 1 - zeroes);

        int step = 0;
        for (int i = 0; i < zeroes; i++)
        {
            //print(i + " step " + (stepSize2 *  i));
            vertices[i] = Vector3.zero;
            _uvs[i] = new Vector2(stepSize2 * (i), 0);
        }


        for (int i = 1 + zeroes; i < points; i++)
        {
            float angle = Mathf.Lerp(-AttackAngle.Value, AttackAngle.Value, step * stepSize);

            angle = angle * Mathf.Deg2Rad;

            float s = Mathf.Sin(angle);
            float c = Mathf.Cos(angle);

            rotatedVector.x = vectorToRotate.x * c - vectorToRotate.z * s;
            rotatedVector.z = vectorToRotate.x * s + vectorToRotate.z * c;



            vertices[i] = rotatedVector;
            _uvs[i] = new Vector2(stepSize * step, 1);
            step++;
        }

        


        ConeMesh.vertices = vertices;
        /*
                if (_uvs.Length != vertices.Length)
                {
                    _uvs = new Vector2[vertices.Length];


                    Bounds bounds = mesh.bounds;

                    int i = 0;
                    while (i < _uvs.Length)
                    {
                        _uvs[i] = new Vector2(vertices[i].x / bounds.size.x, vertices[i].z / bounds.size.z);
                        i++;
                    }

                }*/

        if (ConeMesh.triangles != _triangles)
            ConeMesh.triangles = _triangles;

        if (ConeMesh.normals != _normals)
            ConeMesh.normals = _normals;

        if (ConeMesh.uv != _uvs)
            ConeMesh.uv = _uvs;

        ConeMesh.RecalculateBounds();
    }


    void CreateCone()
    {
        _cone = new GameObject();
        _cone.name = "cone";
        ConeMesh = _cone.AddComponent<MeshFilter>().mesh;
        _coneRenderer = _cone.AddComponent<MeshRenderer>();


        _coneRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        _coneRenderer.receiveShadows = false;



        _coneRenderer.material = AttackMaterial;

        Vector3 offset = PlayerGraphics.position;

        //offset.y = 0.005f;
        _cone.transform.position = offset;

        _cone.transform.rotation = PlayerGraphics.rotation;


        _cone.transform.parent = PlayerGraphics;

        increaseAplha = false;
        



    }
}

