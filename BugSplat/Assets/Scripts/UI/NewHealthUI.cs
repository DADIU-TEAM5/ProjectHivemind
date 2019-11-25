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

    public List<Image> HealthIcons;
    public List<Image> MaxHealthIcons;

    public List<GameObject> MaxHealthBars;
    public List<GameObject> AnimControllers;



    public void Awake()
    {
        //int hearts =
        /*HealthIcons = new List<Image>();
        foreach (Image img in Bar.GetComponentsInChildren<Image>())
            HealthIcons.Add(img);

        MaxHealthIcons = new List<Image>();
        foreach (Image img in MaxBar.GetComponentsInChildren<Image>())
            MaxHealthIcons.Add(img);*/

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
        HealthIcons[fullHearts].transform.localScale = new Vector3(scale, scale, scale);


        for (int i = fullHearts+1; i < HealthIcons.Count; i++)
        {
            HealthIcons[i].enabled = false;
        }

        if (maxHearts != HealthIcons.Count)
        {
            for (int i = maxHearts; i < HealthIcons.Count; i++)
            {
                MaxHealthIcons[i].enabled = false;
            }

            if (maxHearts < 9)
            {
                MaxHealthBars[2].SetActive(false);
                MaxHealthBars[3].SetActive(false);
                MaxHealthBars[4].SetActive(false);
                MaxHealthBars[5].SetActive(false);

                AnimControllers[2].SetActive(false);
                AnimControllers[3].SetActive(false);
                AnimControllers[4].SetActive(false);
                AnimControllers[5].SetActive(false);
            }

            if (maxHearts > 8 && maxHearts < 17)
            {
                MaxHealthBars[2].SetActive(true);
                MaxHealthBars[3].SetActive(true);
                MaxHealthBars[4].SetActive(false);
                MaxHealthBars[5].SetActive(false);

                AnimControllers[2].SetActive(true);
                AnimControllers[3].SetActive(true);
                AnimControllers[4].SetActive(false);
                AnimControllers[5].SetActive(false);
            }

            if (maxHearts > 16)
            {
                MaxHealthBars[2].SetActive(true);
                MaxHealthBars[3].SetActive(true);
                MaxHealthBars[4].SetActive(true);
                MaxHealthBars[5].SetActive(true);

                AnimControllers[2].SetActive(true);
                AnimControllers[3].SetActive(true);
                AnimControllers[4].SetActive(true);
                AnimControllers[5].SetActive(true);
            }
        } else
        {
            MaxHealthBars[2].SetActive(true);
            MaxHealthBars[3].SetActive(true);
            MaxHealthBars[4].SetActive(true);
            MaxHealthBars[5].SetActive(true);

            AnimControllers[2].SetActive(true);
            AnimControllers[3].SetActive(true);
            AnimControllers[4].SetActive(true);
            AnimControllers[5].SetActive(true);
        }

        if (currentHP <= 0)
        {
            HealthIcons[0].enabled = false;
        }
        
    }

}
