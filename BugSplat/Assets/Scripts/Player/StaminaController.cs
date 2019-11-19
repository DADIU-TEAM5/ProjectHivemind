using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaController : GameLoop
{

    public FloatVariable Stamina;
    public FloatVariable MaxStamina;
    public FloatVariable DashCost;
    public FloatVariable StaminaRegen;

    // Used to change effectiveness of dash by .Value %
    public FloatVariable DashPower;

    // Used to determine DashPower depending on current stamina
    public AnimationCurve DashEffectivenessCurve;

    public Text StaminaText;


    public override void LoopLateUpdate(float deltaTime)
    {
        if (Stamina.Value < MaxStamina.Value)
        {
            Stamina.Value = Mathf.Min(MaxStamina.Value, Stamina.Value + StaminaRegen.Value * deltaTime);
        }

        float dashPower = Stamina.Value / MaxStamina.Value;

        DashPower.Value = DashEffectivenessCurve.Evaluate(dashPower);

        //Debug.Log("Dash Update: Current Stamina: " + Stamina.Value + ", Dash Power: " + DashPower.Value);

        int val = (int)Stamina.Value;
        StaminaText.text = val.ToString();

    }

    public override void LoopUpdate(float deltaTime)
    {

    }

    public void OnDash()
    {
        if (Stamina.Value >= DashCost.Value)
            Stamina.Value = Mathf.Max(Stamina.Value - DashCost.Value, 0);
    }


}
