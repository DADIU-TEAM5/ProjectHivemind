using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Effects/Projectile")]
public class ProjectileEffect : Effect
{
    public Effect ProjectileHitEffect;

    public float AngleOffset;

    public float ProjectileSpeed;

    public GameEvent OnCollideEvent, ProjectileActivated;

    public GameObject ProjectilePrefab;

    public ParticleController ProjectileSplash;

    internal ParticleController _particleController;


    public override void Init()
    {
        _particleController = Instantiate(ProjectileSplash);
    }

    public override void DoEffect(GameObject effectTarget = null)
    {
        if (effectTarget == null) {
            Debug.Log("Target was null - ProjectileEffect");
            return;
        }

        var projectile = Instantiate(ProjectilePrefab, effectTarget.transform.position, effectTarget.transform.rotation * Quaternion.Euler(0, AngleOffset, 0));
        var projectileCollidedEffect = projectile.AddComponent<ProjectileCollideEffect>();
        projectileCollidedEffect.CollideEvent = OnCollideEvent;
        projectileCollidedEffect.CollideEffect = ProjectileHitEffect;
        projectileCollidedEffect.ParticleController = _particleController;

        var projectileRigidbody = projectile.GetComponent<Rigidbody>();
        var projectileVelocity = projectileCollidedEffect.transform.forward * ProjectileSpeed;

        projectileRigidbody.velocity = projectileVelocity;
        //_particles.transform.rotation = Quaternion.LookRotation(effectTarget.transform.forward, effectTarget.transform.up) * Quaternion.Euler(0, AngleOffset, 0);

        ProjectileActivated.Raise(projectileCollidedEffect.gameObject);
    }

    internal class ProjectileCollideEffect : MonoBehaviour {
        internal Effect CollideEffect;
        internal GameEvent CollideEvent;

        internal ParticleController ParticleController;

        void OnCollisionEnter(Collision collision)
        {
            ParticleController.MoveTo(gameObject);
            ParticleController.Play();
            ParticleController.InstantiateAfterParts();

            var enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null) {
                CollideEffect.Trigger(enemy.gameObject);
            }

            Destroy(gameObject);
        }
    }
}
