using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu(menuName="Shop/TierPicker")]
public class TierPicker : ScriptableObject
{
    public TierPercent[] TierPercents;

    public int budget;
    public bool IsGauntlet;

    private float _max;

    public void OnEnable() {
        _max = 0f;

        for (var i = 0; i < TierPercents.Length; i++) {
            _max += TierPercents[i].Percent;
        }
    }

    public Tier ChooseTier() {
        var number = Random.Range(0f, _max);

        for (var i = 0; i < TierPercents.Length; i++) {
            var tp = TierPercents[i];

            if (tp.Percent >= number) {
                return tp.Tier;
            }

            number -= tp.Percent;
        }

        Assert.IsTrue(0f >= number);

        return null;
    }

    [System.Serializable]
    public struct TierPercent {
        public Tier Tier;
        public float Percent;
    }
}