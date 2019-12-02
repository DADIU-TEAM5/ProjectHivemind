using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceResolution : MonoBehaviour
{
    public CanvasScaler scaler;

    private void OnEnable()
    {
        Screen.SetResolution( Screen.resolutions[0].width, Screen.resolutions[0].height, true);

        //scaler.referenceResolution = new Vector2(Screen.resolutions[0].width, Screen.resolutions[0].height);
    }


}
