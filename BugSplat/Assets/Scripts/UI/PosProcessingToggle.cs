using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PosProcessingToggle : MonoBehaviour
{

    public GameText PostProcessingOn;
    public GameText PostProcessingOff;

    public TMPro.TextMeshProUGUI TextToChange;

    public BoolVariable PosprocessingToggle;


    private void Start()
    {
        if (PosprocessingToggle.Value)
            TextToChange.text = PostProcessingOn.GetText();
        else
            TextToChange.text = PostProcessingOff.GetText();
    }

    private void OnEnable()
    {
        if (PosprocessingToggle.Value)
            TextToChange.text = PostProcessingOn.GetText();
        else
            TextToChange.text = PostProcessingOff.GetText();
    }


    public void UpdateText()
    {
        if (PosprocessingToggle.Value)
            TextToChange.text = PostProcessingOn.GetText();
        else
            TextToChange.text = PostProcessingOff.GetText();
    }

    public void Click()
    {

        PosprocessingToggle.Value = !PosprocessingToggle.Value;


        if (PosprocessingToggle.Value)
            TextToChange.text = PostProcessingOn.GetText();
        else
            TextToChange.text = PostProcessingOff.GetText();

    }




}
