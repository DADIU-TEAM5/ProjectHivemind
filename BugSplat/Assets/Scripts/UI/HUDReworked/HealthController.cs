using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public HealthPointController[] HealthPoints;

    public FloatVariable CurrentHealth;
    public FloatVariable MaxHealth;

    public FloatVariable HealthPerPoint;

    [PostProcessScene]
    private void BuildAwake() {
        HealthPerPoint.Value = MaxHealth.Max / HealthPoints.Length;
    }

    private void Awake() {
        UpdateHealth();
    }

    public void UpdateHealth() {
        var currentHealth = CurrentHealth.Value;
        var maxHealth = MaxHealth.Value;
        var fullHealthPoint = 0f;

        for (var i = 0; i < HealthPoints.Length; i++) {
            var healthPoint = HealthPoints[i];

            if (maxHealth > fullHealthPoint) {
                healthPoint.ShowPoint();

                if (currentHealth > fullHealthPoint) {
                    var diff = currentHealth - fullHealthPoint;
                    if (diff >= HealthPerPoint.Value) {
                        healthPoint.SetFillAmount(1f);
                    } else {
                        healthPoint.SetFillAmount(diff / HealthPerPoint.Value);
                    }
                } else {
                    healthPoint.SetFillAmount(0f);
                }

                fullHealthPoint += HealthPerPoint.Value;
            } else {
                healthPoint.HidePoint();
            }
        }
    }
}
