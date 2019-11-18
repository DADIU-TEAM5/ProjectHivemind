using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class NewHealthUI : GameLoop
{
    public FloatVariable MaxHealth;
    public FloatVariable CurrentHealth;
    public FloatVariable HealthPerIcon;

    public Image HealthBar;
    public GameObject Bar;

    List<Image> HealthIcons;
    int _activeIcons;

    public void Awake()
    {
        //int hearts =
        HealthIcons = new List<Image>();
        foreach (Image img in Bar.GetComponentsInChildren<Image>())
            HealthIcons.Add(img);

        if (HealthIcons.Count == 0)
            Debug.LogError("No Health Icons Found");
        else
        {
            //ChangeMaxHealth();
            UpdateHealthBar();

            Debug.Log("Active Hearts = " + _activeIcons);
           
        }
            
            

    }

    public override void LoopLateUpdate(float deltaTime)
    {
    }

    // Could Use Events instead of update to optimize performance
    public override void LoopUpdate(float deltaTime)
    {
        //CalculateHealthRatio();
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {

        float currentHP = CurrentHealth.Value;
        int fullHearts = (int)(currentHP / HealthPerIcon.Value);

        float scale = (currentHP % HealthPerIcon.Value)/ HealthPerIcon.Value;

        HealthIcons[fullHearts].transform.localScale = new Vector3(scale, scale, scale);


        for (int i = fullHearts+1; i < HealthIcons.Count; i++)
        {
            HealthIcons[i].enabled = false;
        }

        if(currentHP <= 0)
        {
            HealthIcons[0].enabled = false;
        }
        
    }

}
