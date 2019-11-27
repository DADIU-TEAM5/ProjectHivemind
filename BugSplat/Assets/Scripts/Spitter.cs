using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spitter : Enemy
{
    public GameObject EggProjectile;
    
    public ParticleController SpitParticles;
    public ParticleController SpitParticleSplash;

    public GameObject ProjectileLocation;

    public Animator WormAnimator;
    
    private SpitterStats _spitterStats;
    
    private bool _attacking;
    private float _attackCharge;

    [HideInInspector]
    public bool FullyUnderground;
    

    private float _attackCooldown = 0;

    private float _fleeValue;

    private float _waitForPathCalc;

    private Vector3 _playerAttackPos;

    [Header("Events")]
    public GameEvent AttackEvent;
    
    public GameEvent AttackChargingEvent;

    public GameEvent BurrowEvent;

    public GameEvent EmergeEvent;

    public GameEvent SpitCollideEvent;


    float _percentIncrease;
    float _chargeCLipLength;
    public AnimationClip ChargeAnimation;

    public void Start()
    {
        _spitterStats = (SpitterStats) stats;

        _chargeCLipLength = ChargeAnimation.length;

        Burrow();

        float Increase = _chargeCLipLength - stats.AttackChargeUpTime;

        float percenIncrease = Increase / stats.AttackChargeUpTime;
        _percentIncrease = percenIncrease;
    }




    public override void TakeDamageBehaviour(float damage)
    {
        if (FullyUnderground)
        {
            _currentHealth += damage;
        }

        if(_currentHealth < _spitterStats.FleeThreshold)
        {
            _fleeValue = _spitterStats.FleeTime;
            EndAttack();
            
        }
    }



    public void Burrow()
    {
        if (!FullyUnderground)
        {
            WormAnimator.SetBool("Underground", true);
            BurrowEvent.Raise(gameObject);

            NavMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        }
    }
    public void Emerge()
    {
        if (FullyUnderground)
        {
            WormAnimator.SetBool("Underground", false);
            EmergeEvent.Raise(this.gameObject);

            NavMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        }
    }


    public override void LoopBehaviour(float deltaTime)
    {
        if (_waitForPathCalc > 0)
            _waitForPathCalc -= deltaTime;

        RemoveFromLockedTargetIfNotVisible();

        if (_fleeValue > 0)
            _fleeValue -= deltaTime;

        if (_attackCooldown > 0)
            _attackCooldown -= deltaTime;

        Debug.DrawLine(transform.position, (transform.position + transform.forward), Color.red);

        if (!PlayerDetected)
        {
            Burrow();
            //Renderer.material.color = SetColor(Color.blue);
            DetectThePlayer();
        }
        else if (playerInRangedAttackRange() || _attacking)
        {
            if (_fleeValue <= 0)
            {
                
                Emerge();

                NavMeshAgent.destination = transform.position;

                if (_attackCooldown <= 0 && !FullyUnderground)
                {
                    if (NavMeshAgent.destination != transform.position)
                        NavMeshAgent.destination = transform.position;

                    //Renderer.material.color = SetColor(Color.red);
                    Attack();
                }
                else
                {
                    LookAtPlayer();
                   // Renderer.material.color = SetColor(Color.yellow);

                }
            }
            else
            {
               // Renderer.material.color = SetColor(Color.yellow);
                MoveTowardsThePlayer();
            }
        }
        else
        {
            //Renderer.material.color = SetColor(Color.yellow);
            MoveTowardsThePlayer();
        }
    }

    public override void LoopLateUpdate(float deltaTime) { }

    void LookAtPlayer()
    {
        Vector3 adjustedPlayerPos = PlayerTransform.position;

        adjustedPlayerPos.y = transform.position.y;

        Quaternion diseredRotation= Quaternion.LookRotation(adjustedPlayerPos -transform.position );

        transform.rotation = Quaternion.Lerp(transform.rotation, diseredRotation, Time.deltaTime*5);
    }
    void Attack()
    {
        

        if (_attacking == false)
        {
            AttackChargingEvent.Raise(gameObject);

            Vector3 adjustedPlayerPos = PlayerTransform.position;

            adjustedPlayerPos.y = transform.position.y;
            transform.LookAt(adjustedPlayerPos);

            _playerAttackPos = PlayerTransform.position;

            WormAnimator.speed = 1 + _percentIncrease;

            WormAnimator.SetTrigger("ChargeAttack");

            Outline.SetActive(true);
            Cone.SetActive(true);
            DrawSpitTrajectory();


        }
        DrawSpitFillup();

        ConeRenderer.material.color = Color.Lerp(new Color(0, 1, 0, 0.5f), new Color(1, 0, 0, 0.5f), _attackCharge / stats.AttackChargeUpTime);

        //print(_attackCharge / _spitterStats.AttackChargeUpTime);

        //WormAnimator.SetFloat("ChargeTime", _attackCharge/_spitterStats.AttackChargeUpTime);




        _attacking = true;
        _attackCharge += Time.deltaTime;

        if (_attackCharge >= _spitterStats.AttackChargeUpTime)
        {
            //print("spit emittet");

            Vector3 temppos = _playerAttackPos;
            //temppos.y = ProjectileLocation.transform.position.y;


            ProjectileLocation.transform.LookAt(temppos);

            AttackEvent.Raise(gameObject);

            float roll = Random.Range(0, 100);

            if (_spitterStats.ShootEggs && roll<_spitterStats.ChanceToShootEgg)
            {
                GameObject egg = Instantiate(EggProjectile, ProjectileLocation.transform);

                egg.transform.parent = null;

                EggShell eggScript = egg.GetComponent<EggShell>();

                eggScript.Damage = _spitterStats.AttackDamage;

                eggScript.setSpeed(_spitterStats.ProjectileSpeed);
                eggScript.SetLifeTime(_spitterStats.AttackRange / _spitterStats.ProjectileSpeed);

            }
            else
            {
                var spit = MakeSpit();
            }

            _attackCooldown = _spitterStats.AttackSpeed;
            _attacking = false;
            _attackCharge = 0;
            WormAnimator.speed = 1;

            Outline.SetActive(false);
            Cone.SetActive(false);

        }

    }


    void EndAttack()
    {
        _attackCooldown = _spitterStats.AttackSpeed;
        _attacking = false;
        _attackCharge = 0;
        WormAnimator.speed = 1;

        Outline.SetActive(false);
        Cone.SetActive(false);
    }
    

    void MoveTowardsThePlayer()
    {
        Burrow();

        if (FullyUnderground && _waitForPathCalc<=0)
        {
            Vector3 adjustedPlayerPos = PlayerTransform.position;

            adjustedPlayerPos.y = transform.position.y;

            Vector3 destination = Vector3.zero;

            destination = adjustedPlayerPos+((transform.position-adjustedPlayerPos).normalized*_spitterStats.AttackRange);

            if (NavMeshAgent.destination != destination)
                NavMeshAgent.destination = destination;

            _waitForPathCalc = .5f;
        }
    }


    int[] _trianglesfortraj = { };
    Vector3[] _normalsfotraj = { };

    public void DrawSpitTrajectory()
    {
        if (_trianglesfortraj.Length != 6)
        {
            _trianglesfortraj = new int[6];

            

            _trianglesfortraj[0] = 0;
            _trianglesfortraj[1] = 1;
            _trianglesfortraj[2] = 2;

            _trianglesfortraj[3] = 2;
            _trianglesfortraj[4] = 1;
            _trianglesfortraj[5] = 3;



        }

        if (_normalsfotraj.Length != 4)
        {

            _normalsfotraj = new Vector3[4];

            for (int i = 0; i < 4; i++)
            {
                _normalsfotraj[i] = Vector3.up;
            }
        }

        Vector3[] vertices = new Vector3[4];

        float trajectoryWidt = 0.5f;

        vertices[0] =  Vector3.right * trajectoryWidt;
        vertices[1] =  Vector3.left * trajectoryWidt;

        vertices[2] =  (Vector3.right* trajectoryWidt) + (Vector3.forward * stats.AttackRange);
        vertices[3] =  (Vector3.left* trajectoryWidt) + (Vector3.forward * stats.AttackRange);

        OutlineMesh.vertices = vertices;

        if (OutlineMesh.triangles != _trianglesfortraj)
            OutlineMesh.triangles = _trianglesfortraj;

        if (OutlineMesh.normals != _normalsfotraj)
            OutlineMesh.normals = _normalsfotraj;

        OuterEdgeMesh.RecalculateBounds();
    }


    public void DrawSpitFillup()
    {
        if (_trianglesfortraj.Length != 6)
        {
            _trianglesfortraj = new int[6];



            _trianglesfortraj[0] = 0;
            _trianglesfortraj[1] = 1;
            _trianglesfortraj[2] = 2;

            _trianglesfortraj[3] = 2;
            _trianglesfortraj[4] = 1;
            _trianglesfortraj[5] = 3;



        }

        if (_normalsfotraj.Length != 4)
        {

            _normalsfotraj = new Vector3[4];

            for (int i = 0; i < 4; i++)
            {
                _normalsfotraj[i] = Vector3.up;
            }
        }

        Vector3[] vertices = new Vector3[4];

        float trajectoryWidt = 0.5f;

        vertices[0] = Vector3.right * trajectoryWidt;
        vertices[1] = Vector3.left * trajectoryWidt;


        vertices[2] = (Vector3.right * trajectoryWidt) + (Vector3.forward * Mathf.Lerp(0, stats.AttackRange,_attackCharge/stats.AttackChargeUpTime));
        vertices[3] = (Vector3.left * trajectoryWidt) + (Vector3.forward * Mathf.Lerp(0, stats.AttackRange, _attackCharge / stats.AttackChargeUpTime));

        ConeMesh.vertices = vertices;

        if (ConeMesh.triangles != _trianglesfortraj)
            ConeMesh.triangles = _trianglesfortraj;

        if (ConeMesh.normals != _normalsfotraj)
            ConeMesh.normals = _normalsfotraj;

        ConeMesh.RecalculateBounds();
    }

    private ParticleController MakeSpit() {
        var spitObj = Instantiate(SpitParticles, ProjectileLocation.transform.position, ProjectileLocation.transform.rotation);

        var projectileCollide = spitObj.gameObject.AddComponent<SpitterProjectileCollide>();
        projectileCollide.ParticleController = SpitParticleSplash;
        projectileCollide.CollideEvent = SpitCollideEvent;
        projectileCollide.Damage = _spitterStats.AttackDamage;
        projectileCollide.Range = _spitterStats.AttackRange;
        Debug.Log("helloooo");

        var spitRB = spitObj.GetComponent<Rigidbody>();
        spitRB.velocity = this.transform.forward * _spitterStats.ProjectileSpeed;

        return spitObj;
    }

    internal class SpitterProjectileCollide : MonoBehaviour {
        internal GameEvent CollideEvent;

        internal float Damage;

        internal float Range = 1000f;

        internal ParticleController ParticleController;

        private ParticleController _particleController;

        private Vector3 _startPos;
        

        void Update() {
            // Using square magnitude for optimizations purposes
            if ((transform.position - _startPos).sqrMagnitude > Range * Range) {
                Destroy(gameObject);
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            _particleController = Instantiate(ParticleController);
            _particleController.MoveTo(gameObject);
            _particleController.Play();
            _particleController.InstantiateAfterParts();

            var player = collision.gameObject.GetComponent<PlayerHealth>();
            if (player != null) {
                player.TakeDamage(Damage);
            }

            Destroy(_particleController, 3);
            Destroy(gameObject);
        }
    }

    public override void SpawnFromUnderground()
    {

    }
}
