using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPointController : MonoBehaviour
{
    public Canvas Canvas;
    public Image HealthImage;

    public void ShowPoint() {
        Canvas.enabled = true;
    }
    
    public void HidePoint() {
        Canvas.enabled = false;
    }

    public void SetFillAmount(float fillAmount) {
        HealthImage.fillAmount = fillAmount;
    }
}