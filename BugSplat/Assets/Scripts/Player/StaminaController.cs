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
    public List<GameObject> MaxStaminaIcons;
    public FloatVariable StaminaMaxSize;
    private float _staminaIconOffset = 27;
    private float _staminaDistFromWall = 40;
    private int _staminaCharges;

    private void OnEnable()
    {
        StaminaMaxSize.ResetValue();
        float staminaCharges = MaxStamina.Value / DashCost.Value;
        _staminaCharges = (int) (Stamina.Value / DashCost.Value);
        StaminaMaxSize.Value = staminaCharges/StaminaIcons.Count * StaminaMaxSize.Value;
        
        Debug.Log("StaminaSize: " + StaminaMaxSize.Value);

        for (int i = 0; i < MaxStaminaIcons.Count; i++)
        {
            if (i < staminaCharges)
                MaxStaminaIcons[i].SetActive(true);
            else
                MaxStaminaIcons[i].SetActive(false);
        }
    }

    public override void LoopLateUpdate(float deltaTime)
    {
        int newStaminaChagrges = (int)(Stamina.Value / DashCost.Value);
        if (newStaminaChagrges > _staminaCharges)
        {
            StaminaIcons[_staminaCharges].SetActive(false);
            StaminaIcons[_staminaCharges].SetActive(true);
            _staminaCharges = newStaminaChagrges;
        }
        if (Stamina.Value < MaxStamina.Value)
        {
            Stamina.Value = Mathf.Min(MaxStamina.Value, Stamina.Value + StaminaRegen.Value * deltaTime);
            if (Stamina.Value < 0)
                Stamina.Value = 0;
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

        for (int i = 0; i < StaminaIcons.Count; i++)
        {
            StaminaIcons[i].SetActive(false);
            StaminaIcons[i].SetActive(true);
        }

        _staminaCharges--;
    }


}
