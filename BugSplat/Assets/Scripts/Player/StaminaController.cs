using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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



    public override void LoopLateUpdate(float deltaTime)
    {
        if(Stamina.Value < MaxStamina.Value)
        {
            Stamina.Value = Mathf.Min(MaxStamina.Value, Stamina.Value + StaminaRegen.Value);
        }

        float dashEffect = Stamina.Value / MaxStamina.Value;

        //DashPower.Value =
    }

    public override void LoopUpdate(float deltaTime)
    {
     
    }


}
