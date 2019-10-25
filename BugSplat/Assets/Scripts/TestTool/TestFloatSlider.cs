using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestFloatSlider : MonoBehaviour
{

    public FloatVariable FloatVar;
    public Text TextLabel;
    private string _textLabel;
    public Slider slider;
    //public FloatVariable FloatVar;

    public void Setup(FloatVariable floatVar)
    {
        FloatVar = floatVar;
        slider.value = FloatVar.Value;
        SetText();
    }

    public void ChangeSlider()
    {
        FloatVar.Value = slider.value;
        SetText();
    }

    public void SetText()
    {
        _textLabel = FloatVar.name + ": " + FloatVar.Value;
        TextLabel.text = _textLabel;
    }
}
