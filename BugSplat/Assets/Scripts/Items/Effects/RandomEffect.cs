using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Effects/Random")]
public class RandomEffect : Effect
{
    public List<Effect> Effects;

    public override void Init()
    {
        foreach (var effect in Effects) {
            effect.Init();
        }
    }

    public override void Trigger(GameObject target = null)
    {
        if (Effects.Count == 0) return;

        var randomEffect = Random.Range(0, Effects.Count);
        Debug.Log(randomEffect);

        var chosenEffect = Effects[randomEffect];
        chosenEffect.Trigger(target);
    }
}
