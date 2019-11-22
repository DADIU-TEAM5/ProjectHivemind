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

    public int[] WaveBudgets;

    private float _max;

    private int _selectedTier = -1;

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
                _selectedTier = i;
                return tp.Tier;
            }

            number -= tp.Percent;
        }

        Assert.IsTrue(0f >= number);

        return null;
    }

    public Tier NextTier() {
        var nextTier = (_selectedTier + 1) % TierPercents.Length;

        return TierPercents[nextTier].Tier;
    }

    [System.Serializable]
    public struct TierPercent {
        public Tier Tier;
        public float Percent;
    }
}