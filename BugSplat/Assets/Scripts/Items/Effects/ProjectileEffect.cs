using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Effects/Projectile")]
public class ProjectileEffect : Effect
{
    public Effect ProjectileHitEffect;

    public Vector3Variable PlayerDirection;

    public GameObject Particles;
    private ParticleSystem _particleSystem;

    private GameObject _particles;

    public override void Init()
    {
        _particles = Instantiate(Particles);
        _particleSystem = _particles.GetComponentInChildren<ParticleSystem>();

        // TODO: DUMB DEBUG HACK RIGHT NOW
        var spitProjectile = _particles.GetComponent<SpitProjectile>();
        if (spitProjectile != null) Destroy(spitProjectile);
        // --------------------------------------------------

        var projectileCollide = _particles.AddComponent<ProjectileCollideEffect>();
        projectileCollide.CollideEffect = ProjectileHitEffect;

        _particles.layer = (1 << 11);
    }

    public override void Trigger(GameObject effectTarget = null)
    {

        Debug.Log("PROJECTILES YO");
        _particles.transform.position = effectTarget.transform.position;
        _particles.transform.rotation = Quaternion.LookRotation(PlayerDirection.Value, effectTarget.transform.up);
        _particles.transform.position += PlayerDirection.Value;

        _particleSystem.Emit(1);
    }

    internal class ProjectileCollideEffect : MonoBehaviour {
        internal Effect CollideEffect;

        private void OnParticleCollision(GameObject other)
        {
            Debug.Log("COLLIDING");
            CollideEffect.Trigger(other);
        }
    }
}
