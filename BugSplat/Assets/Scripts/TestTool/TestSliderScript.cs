using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSliderScript : MonoBehaviour
{
    public IntVariable IntVar;
    public Text text;
    private string _textLabel;
    public Slider slider;
    //public FloatVariable FloatVar;
    
    public void Setup(IntVariable intVar) 
    {
        IntVar = intVar;
        slider.value = IntVar.Value;
        SetText();
    }

    public void ChangeSlider()
    {
        IntVar.Value = (int)slider.value;
        SetText();
    }

    public void SetText()
    {
        _textLabel = text.text + ": " + IntVar.Value;
    }
}
