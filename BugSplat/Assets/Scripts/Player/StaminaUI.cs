using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    public RectTransform staminaTransform;
    private float initTransformWidth;
    private Image staminaImage;
    public FloatVariable CurrentStaminaSO;
    public FloatVariable DashCostSO;
    public Color ActiveStaminaColor;
    public Color InactiveStaminaColor;
    
    // Start is called before the first frame update
    void Start()
    {
        //staminaTransform = this.GetComponent<RectTransform>();
        initTransformWidth = staminaTransform.sizeDelta.x;
        staminaImage = staminaTransform.gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        staminaTransform.sizeDelta = new Vector2((CurrentStaminaSO.Value / CurrentStaminaSO.InitialValue) * initTransformWidth, staminaTransform.sizeDelta.y);

        if (CurrentStaminaSO.Value < DashCostSO.Value)
        {
            staminaImage.color = InactiveStaminaColor;
        } else
        {
            staminaImage.color = ActiveStaminaColor;
        }
    }
}
