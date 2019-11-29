using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Effects/Timebased StatChange")]
public class TimeBasedStatChange : Effect
{
    public StatChangeEffect StatChange;

    public GameEvent BoostInit, BoostOver;

    public float Timer;

    private EmptyMono CoroutineBoy;

    public override void Init()
    {
        CoroutineBoy = MakeCoroutineObject();
        CoroutineBoy.gameObject.name = "TimeBasedStatChange_CoroutineBoy";
    }

    public override void DoEffect(GameObject target = null)
    {
        CoroutineBoy.StartCoroutine(EffectCountdown());
    }

    private IEnumerator EffectCountdown() {
        float actualChange = 0f;
        if (StatChange.Change > 0) {
            var change = StatChange.GetMax() - StatChange.Stat.Value;
            actualChange = Mathf.Min(change, StatChange.Change);
        } else {
            var change = 0 - StatChange.Stat.Value;
            actualChange = Mathf.Max(change, StatChange.Change);
        }

        BoostInit?.Raise();

        StatChange.Stat.SetValue(StatChange.Stat.Value + actualChange);

        yield return new WaitForSeconds(Timer);

        BoostOver?.Raise();

        StatChange.Stat.SetValue(StatChange.Stat.Value - actualChange);
    }
}