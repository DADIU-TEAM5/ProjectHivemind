using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Effects/Projectile")]
public class ProjectileEffect : Effect
{
    public Effect ProjectileHitEffect;

    public float AngleOffset;

    public GameEvent OnCollideEvent;

    public GameObject Particles;
    private ParticleSystem _particleSystem;

    private GameObject _particles;

    public override void Init()
    {
        _particles = Instantiate(Particles);
        _particleSystem = _particles.GetComponentInChildren<ParticleSystem>();

        var projectileCollide = _particles.AddComponent<ProjectileCollideEffect>();
        projectileCollide.CollideEffect = ProjectileHitEffect;
        projectileCollide.CollideEvent = OnCollideEvent;

        _particles.layer = (1 << 11);
    }

    public override void Trigger(GameObject effectTarget = null)
    {

        _particles.transform.position = effectTarget.transform.position;
        _particles.transform.rotation = Quaternion.LookRotation(effectTarget.transform.forward, effectTarget.transform.up) * Quaternion.Euler(0, AngleOffset, 0);

        _particleSystem.Emit(1);
    }

    internal class ProjectileCollideEffect : MonoBehaviour {
        internal Effect CollideEffect;
        internal GameEvent CollideEvent;

        private void OnParticleCollision(GameObject other)
        {
            CollideEffect.Trigger(other);
            CollideEvent?.Raise(other);
        }
    }
}
