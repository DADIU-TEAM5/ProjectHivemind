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
    public GameObject MaxBar;

    List<Image> HealthIcons;
    List<Image> MaxHealthIcons;


    public void Awake()
    {
        //int hearts =
        HealthIcons = new List<Image>();
        foreach (Image img in Bar.GetComponentsInChildren<Image>())
            HealthIcons.Add(img);

        MaxHealthIcons = new List<Image>();
        foreach (Image img in MaxBar.GetComponentsInChildren<Image>())
            MaxHealthIcons.Add(img);

        if (HealthIcons.Count == 0)
            Debug.LogError("No Health Icons Found");
        else
        {
            //ChangeMaxHealth();
            UpdateHealthBar();

           
           
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
        for (int i = 0; i < HealthIcons.Count; i++)
        {
            MaxHealthIcons[i].enabled = true;
            MaxHealthIcons[i].transform.localScale = new Vector3(1, 1, 1);
            HealthIcons[i].enabled = true;
            HealthIcons[i].transform.localScale = new Vector3(1, 1, 1);
        }

        float currentHP = CurrentHealth.Value;
        int fullHearts = (int)(currentHP / HealthPerIcon.Value);
        int maxHearts = (int)((1+MaxHealth.Value) / HealthPerIcon.Value);

        //Debug.Log("Max HP: " + MaxHealth.Value + ", HealthPrIcon: " +HealthPerIcon.Value +", maxHearts: " + maxHearts);

        float scale = (currentHP % HealthPerIcon.Value)/ HealthPerIcon.Value;

        if (scale > 0 && scale < 0.9f)
            scale = 0.5f;

        //Debug.Log("HP SCALE: " + scale);
        //HealthIcons[fullHearts].transform.localScale = new Vector3(scale, scale, scale);


        for (int i = fullHearts+1; i < HealthIcons.Count; i++)
        {
            HealthIcons[i].enabled = false;
        }

        for (int i = maxHearts; i < HealthIcons.Count; i++)
        {
            MaxHealthIcons[i].enabled = false;
        }

        if (currentHP <= 0)
        {
            HealthIcons[0].enabled = false;
        }
        
    }

}
