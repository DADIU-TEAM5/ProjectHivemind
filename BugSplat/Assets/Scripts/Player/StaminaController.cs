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

    // Used to get size
    public RectTransform StaminaPrefab;
    public RectTransform StaminaMask;
    public List<GameObject> StaminaIcons;
    public FloatVariable StaminaMaxSize;
    private float _staminaIconOffset = 77;
    private float _staminaDistFromWall = 30;

    private void OnEnable()
    {
        StaminaMaxSize.ResetValue();
        float staminaCharges = MaxStamina.Value / DashCost.Value;
        StaminaMaxSize.Value = staminaCharges/StaminaIcons.Count * StaminaMaxSize.Value;
        
        Debug.Log("StaminaSiza: " + StaminaMaxSize.Value);
    }

    public override void LoopLateUpdate(float deltaTime)
    {
        if (Stamina.Value < MaxStamina.Value)
        {
            Stamina.Value = Mathf.Min(MaxStamina.Value, Stamina.Value + StaminaRegen.Value * deltaTime);
        }
       
        float staminaPercent = Stamina.Value / MaxStamina.Value;
        Debug.Log("Stamina % : " + staminaPercent);
        // UI
        StaminaMask.sizeDelta = new Vector2(staminaPercent*StaminaMaxSize.Value, StaminaMask.rect.height);

        float dashPower = Stamina.Value / MaxStamina.Value;

        DashPower.Value = DashEffectivenessCurve.Evaluate(dashPower);

        //Debug.Log("Dash Update: Current Stamina: " + Stamina.Value + ", Dash Power: " + DashPower.Value);

        //int val = (int)Stamina.Value;
        //StaminaText.text = val.ToString();

    }

    public override void LoopUpdate(float deltaTime)
    {

    }

    public void OnDash()
    {
        //StaminaMask.sizeDelta = new Vector2(StaminaMask.rect.width - _staminaIconOffset, StaminaMask.rect.height);
        if (Stamina.Value >= DashCost.Value)
            Stamina.Value = Mathf.Max(Stamina.Value - DashCost.Value, 0);
    }


}
