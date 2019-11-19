using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName="Effects/Slow Enemy")]
public class SlowEnemyEffect : Effect
{
    public float Change;

    public float Duration = 3;

    private EmptyMono CoroutineBoy;

    public override void Init()
    {
        CoroutineBoy = MakeCoroutineObject();
        CoroutineBoy.name = "CoroutineBoy - SlowEnemyEffect";
    }

    public override void DoEffect(GameObject target = null)
    {
        var navmeshagent = target?.GetComponent<NavMeshAgent>();
        if (navmeshagent == null) return;

        CoroutineBoy.StartCoroutine(SlowEnemy(navmeshagent));
    }

    private IEnumerator SlowEnemy(NavMeshAgent agent) {
        var agentSpeed = agent.speed;

        agent.speed += Change;

        yield return new WaitForSeconds(Duration);

        agent.speed = agentSpeed;
    }
}
