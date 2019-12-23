using UnityEngine;

public class PlayerStamina : GameLoop
{
    public FloatVariable CurrentStamina;
    public FloatVariable MaxStamina;
    public FloatVariable DashCost;
    public FloatVariable StaminaRegen;

    // Used to change effectiveness of dash by .Value %
    public FloatVariable DashPower;

    // Used to determine DashPower depending on current stamina
    public AnimationCurve DashEffectivenessCurve;

    [Header("Events")]
    public GameEvent NotEnoughStaminaForDash;

    public override void LoopUpdate(float deltaTime)
    {
        if (CurrentStamina.Value < MaxStamina.Value) {
            CurrentStamina.Value += StaminaRegen.Value * deltaTime;
        }
        
        // Sanitize the stamina
        if (CurrentStamina.Value < 0) CurrentStamina.Value = 0;
        if (CurrentStamina.Value > MaxStamina.Value) CurrentStamina.Value = MaxStamina.Value;
    }

    public void OnDash()
    {   
        var dashPower = CurrentStamina.Value / MaxStamina.Value;
        DashPower.Value = DashEffectivenessCurve.Evaluate(dashPower);

        if (CurrentStamina.Value >= DashCost.Value)
            CurrentStamina.Value -= DashCost.Value;
    }

    public override void LoopLateUpdate(float deltaTime) {}
}
