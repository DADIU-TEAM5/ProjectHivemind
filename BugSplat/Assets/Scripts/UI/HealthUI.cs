using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class HealthUI : GameLoop
{
    public FloatVariable MaxHealth;
    public FloatVariable CurrentHealth;

    public Image HealthBar;

    private float _healthRatio;

    public override void LoopLateUpdate(float deltaTime)
    {
    }

    public override void LoopUpdate(float deltaTime) {
        CalculateHealthRatio();
        UpdateHealthBar();
    }

    private void UpdateHealthBar() {
        HealthBar.fillAmount = _healthRatio;
    }

    private void CalculateHealthRatio() {
        var healthRatio = CurrentHealth.Value / MaxHealth.Value;
        // Normalize it
        if (healthRatio < 0) healthRatio = 0f;
        if (healthRatio > 1) healthRatio = 1f;

        _healthRatio = healthRatio;
    }
}
