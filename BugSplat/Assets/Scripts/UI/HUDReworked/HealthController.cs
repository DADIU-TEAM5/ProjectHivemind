using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;

public class HealthController : MonoBehaviour, IPreprocessBuildWithReport
{
    public HealthPointController[] HealthPoints;

    public FloatVariable CurrentHealth;
    public FloatVariable MaxHealth;

    public FloatVariable HealthPerPoint;

    public int callbackOrder => 2000;

    private void Awake() {
        #if UNITY_EDITOR
            OnPreprocessBuild(null);
        #endif
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

    public void OnPreprocessBuild(BuildReport report)
    {
        HealthPerPoint.Value = MaxHealth.Max / HealthPoints.Length;
    }
}
